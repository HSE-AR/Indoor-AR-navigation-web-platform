<template>
<div>
   <header>
      <details open>
        <summary>Immersive AR Session</summary>
        <p>
          This sample demonstrates how to use an 'immersive-ar' XRSession to
          present a simple WebGL scene to an transparent or passthrough XR
          device. The logic is largely the same as the corresponding VR sample,
          with the primary difference being that no background is rendered and
          the model is scaled down for easier viewing in a real-world space.
          <a class="back" href="./">Back</a>
        </p>
      </details>
    </header>
</div>

</template>



<script>
import {THREE} from '../main.js'
import {WebXRButton} from '../../lib/util/webxr-button'
import {Scene} from '../../lib/render/scenes/scene.js';
import {Renderer, createWebGLContext} from '../../lib/render/core/renderer.js';
import {Gltf2Node} from '../../lib/render/nodes/gltf2.js';
import {Gltf2Loader as Gltf2LoaderWebXR} from '../../lib/render/loaders/gltf2.js';
import {QueryArgs} from '../../lib/util/query-args.js';
import {mat4, vec3, quat} from '../../lib/render/math/gl-matrix.js'; 
import {Node} from '../../lib/render/core/node.js';
import {PbrMaterial} from '../../lib/render/materials/pbr.js';
import {InlineViewerHelper} from '../../lib/util/inline-viewer-helper.js';
import { GLTFExporter } from '../../public/threejs/examples/jsm/exporters/GLTFExporter'

import WebXRPolyfill from '../../lib/third-party/webxr-polyfill/build/webxr-polyfill.module.js';

import axios from 'axios'
import router from '../router'
import { GLTFLoader } from '../../public/threejs/examples/jsm/loaders/GLTFLoader.js';


export default {
  data(){
    return{
      xrButton:null,
      xrSession: null,
      xrImmersiveRefSpace: null,
      gl:null,
      scene:new Scene(),
      renderer:null,
      inlineViewerHelper:null
    }
  },
  async mounted(){
        await this.initXR();

  },

  methods:{
    
    async LoadSceneFromBack(renderer){
        const vm = this;
      
      	await axios.get(this.$store.state.port +'/api/models/'+this.$store.state.sceneTestId).then(response =>{

            var loader = new THREE.ObjectLoader();

				    loader.parse( response.data.scene, function ( resultScene ) {

                var exporter = new GLTFExporter();

                exporter.parse( resultScene, function ( resultGltf ) {

                    let gltf2LoaderWebXR = new Gltf2LoaderWebXR(renderer);
                    let sceneFromBack = gltf2LoaderWebXR.loadFromJson(resultGltf)
                    sceneFromBack.scale = [0.1, 0.1, 0.1]
                    vm.scene.addNode(sceneFromBack)
                });
            });
        })
    },


    initXR() {
        this.xrButton = new WebXRButton({
          onRequestSession: this.onRequestSession,
          onEndSession: this.onEndSession,
          textEnterXRTitle: "START AR",
          textXRNotFoundTitle: "AR NOT FOUND",
          textExitXRTitle: "EXIT  AR",
        });
        document.querySelector('header').appendChild(this.xrButton.domElement);

        if (navigator.xr) {
          navigator.xr.isSessionSupported('immersive-ar').then((supported) => {
            this.xrButton.enabled = supported;
          });

          navigator.xr.requestSession('inline').then(this.onSessionStarted);
        }
    },

    onRequestSession() {
        return navigator.xr.requestSession('immersive-ar')
            .then((session) => {
              this.xrButton.setSession(session);
              session.isImmersive = true;
              this.onSessionStarted(session);
            });
    },

    async initGL() {
        if (this.gl)
            return;

          this.gl = createWebGLContext({
            xrCompatible: true
          });
          document.body.appendChild(this.gl.canvas);

          function onResize(gl) {
            gl.canvas.width = gl.canvas.clientWidth * window.devicePixelRatio;
            gl.canvas.height = gl.canvas.clientHeight * window.devicePixelRatio;
          }
          window.addEventListener('resize', onResize);
          onResize(this.gl);

          this.renderer = new Renderer(this.gl);

          await this.LoadSceneFromBack(this.renderer);

          this.scene.setRenderer(this.renderer);
    },


    async onSessionStarted(session) {
        session.addEventListener('end', this.onSessionEnded);

        await this.initGL();
         session.updateRenderState({ baseLayer: new XRWebGLLayer(session, this.gl) });

        let refSpaceType = session.isImmersive ? 'local' : 'viewer';
        session.requestReferenceSpace(refSpaceType).then((refSpace) => {
          if (session.isImmersive) {
            this.xrImmersiveRefSpace = refSpace;
          } else {
            this.inlineViewerHelper = new InlineViewerHelper(this.gl.canvas, refSpace);
          }
          session.requestAnimationFrame(this.onXRFrame);
        });
    },

    onEndSession(session) {
        session.end();
    },

    onSessionEnded(event) {
        if (event.session.isImmersive) {
          this.xrButton.setSession(null);
        }
    },

    onXRFrame(t, frame) {
        let session = frame.session;
        let refSpace = session.isImmersive ?
                         this.xrImmersiveRefSpace :
                         this.inlineViewerHelper.referenceSpace;
        let pose = frame.getViewerPose(refSpace);

        this.scene.startFrame();

        session.requestAnimationFrame(this.onXRFrame);

        this.scene.drawXRFrame(frame, pose);

        this.scene.endFrame();
    },

  }
}
</script>
