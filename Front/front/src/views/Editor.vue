<template>
  <div>

  </div>
</template>


<script>

import { THREE,Editor,Viewport,Toolbar,Player,Sidebar,Menubar,Resizer} from '../main.js'
import { Loader } from '../../public/threejs/build/three.module.js';

import axios from 'axios'

Number.prototype.format = function () {
	return this.toString().replace( /(\d)(?=(\d{3})+(?!\d))/g, "$1," );

};


export default {
  name: 'Editor',
  data(){
      return{
          workspace: null,
          editor:null,
          viewport:null,
          toolbar:null,
          sidebar:null,
          menubar:null,
          resizer:null,
          isLoadingFromHash: false,
          hash:null,
          
      }
  },

  async created(){
	
    window.URL = window.URL || window.webkitURL;
    window.BlobBuilder = window.BlobBuilder || window.WebKitBlobBuilder || window.MozBlobBuilder;
    
    this.editor = new Editor();
    window.editor = this.editor; // Expose editor to Console
	window.THREE = THREE; // Expose THREE to APP Scripts and Console


    
    this.viewport = new Viewport( this.editor );
    document.body.appendChild( this.viewport.dom );

	this.toolbar = new Toolbar( this.editor );
    document.body.appendChild( this.toolbar.dom );
      

	this.sidebar = new Sidebar( this.editor );
	document.body.appendChild( this.sidebar.dom );

	this.menubar = new Menubar( this.editor );
	document.body.appendChild( this.menubar.dom );

    this.resizer = new Resizer( this.editor );
	document.body.appendChild( this.resizer.dom );
	


    /*this.editor.storage.init( function () {

		editor.storage.get( function ( state ) {
            if ( isLoadingFromHash ) 
                return;
            
			if ( state !== undefined ) {
				editor.fromJSON( state );
			}
            var selected = editor.config.getKey( 'selected' );

			if ( selected !== undefined ) {
				editor.selectByUuid( selected );
			}
		});
		//
		var timeout;

		function saveState() {

			
			console.log(editor.toJSON()["scene"] = {"test" : "qwerty"})
			console.log(editor.toJSON()["scene"])

					/*( editor.config.getKey( 'autosave' ) === false ) {
						return;
					}
					clearTimeout( timeout );
					timeout = setTimeout( function () {
						editor.signals.savingStarted.dispatch();
						timeout = setTimeout( function () {
							editor.storage.set( editor.toJSON() );
							editor.signals.savingFinished.dispatch();
						}, 100 );
					}, 1000 );*/

		/*}

		var signals = editor.signals;

		signals.geometryChanged.add( saveState );
		signals.objectAdded.add( saveState );
		signals.objectChanged.add( saveState );
		signals.objectRemoved.add( saveState );
		//signals.materialChanged.add( saveState );
		signals.sceneBackgroundChanged.add( saveState );
		signals.sceneFogChanged.add( saveState );
		signals.sceneGraphChanged.add( saveState );
		//signals.scriptChanged.add( saveState );
		signals.historyChanged.add( saveState );
	} );*/
    
    document.addEventListener( 'dragover', function ( event ) {
		event.preventDefault();
		event.dataTransfer.dropEffect = 'copy';
	}, false );

	document.addEventListener( 'drop', function ( event ) {
		event.preventDefault();
        if ( event.dataTransfer.types[ 0 ] === 'text/plain' ) 
            return; // Outliner drop

		if ( event.dataTransfer.items ) {
					// DataTransferItemList supports folders
			this.editor.loader.loadItemList( event.dataTransfer.items );
		} else {
			this.editor.loader.loadFiles( event.dataTransfer.files );
        }   
	}, false );


	window.addEventListener( 'resize', this.onWindowResize, false );

    this.onWindowResize();
            
    var isLoadingFromHash = false;
	this.hash = window.location.hash;

	await this.LoadSceneFromBack();	
  },

  destroyed(){
    location.reload()
  },

  methods:{

	onWindowResize()
	{
        this.editor.signals.windowResize.dispatch();
	},
	
	async LoadSceneFromBack()
	{
      	await axios.get(this.$store.state.port +'/api/models/'+this.$store.state.sceneTestId).then(response =>{
			console.log(response)
			this.editor.idFromBack = response.data.id;
			this.editor.loader.MyLoader(response.data.scene);

			this.editor.select( null );
        })
	},  

  }

}
</script>
