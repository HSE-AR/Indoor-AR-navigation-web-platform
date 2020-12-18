import * as THREE from '../../build/three.module.js';

import { UIPanel, UIRow } from './libs/ui.js';
import axios from 'axios'

function MenubarSaveAndExit( editor ) { //моеееее

    console.log("Examples нам не нужны")
    
	var strings = editor.strings;

	var container = new UIPanel();
	container.setClass( 'menu' );

	var title = new UIPanel();
	title.setClass( 'title' );
    title.setTextContent( "SAVE" ); //потом надо будет завести строку в  strings 

    title.onClick( async function () {
        // пока только в тестовом режиме

        console.log(editor.idFromBack)
        console.log(editor.history.undos[1])
        var modification = editor.history.undos[1];

        const data ={
            'modelId':editor.idFromBack,
            'id':modification.json.objectUuid,
            'type':modification.json.type,
            'object':modification.object.toJSON()
        }
        console.log(data)
        var port = 'http://localhost:5555'
      	//var config ={headers:{ Authorization :"Bearer "+ this.state.AllAboutToken.accessToken}}
      	/*const data={
        'itemId':item.id,
        'star':item.top,
        'userId':this.state.user.sub}*/
      	/*await axios.post(port +'/api/test').then(response =>{
			console.log(response)

        })*/
	} );
	container.add( title );

	

	return container;

}

export { MenubarSaveAndExit };
