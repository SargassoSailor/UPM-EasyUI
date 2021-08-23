![Easy UI Builder](https://chrisashtear.github.io/EUI.png)

<h2>Introduction</h2>

Easy UI is a unity package made for making menus in games easier. It allows you to generate buttons using some simple properties and use a dropdown to select what you want the button to do.

<h2>Populate Buttons</h2>

Putting the popButtons script in a ui gameobject with a layout group will allow you to setup menu buttons easily and change them in one spot if need be. In popbuttons there are 3 properties:

LayoutGroup : the object that will contain the buttons(if its not the object the script is in)

Prefab :  the prefab that will be used as a template for the buttons. One is provided in the package(or in samples if you are using the UPM package)

![Easy UI Builder](https://chrisashtear.github.io/docs/eui/eui1.PNG)

Props : a list of button properties for each button.
* Name:  the text on the button
* Button Color: color of button background
* Label Color: color of the text
* Button Type: A list of choices for what you want that button to do.
  * Change Menu: - switch to a different UI screen, complete with animation. Type the name of the menu in the 'Panel Name' box
  * Start Game: - start gameplay, menu disappears with animation
  * Quit: - exits game
  * Go Back: - go back to previous menu
  * Open Web: - open a web link in a browser
  * Popup Menu: - open a menu without animation and without disabling the previous menu
  * Stop Game: - a 'back to the menu' button.
  * Set Pref: - set a unity preference that you name
* Custom Sound: sound to play(from a list in project settings) when button is clicked.
* Panel Name/Pref Name/URL - the name of the menu object if you are using changemenu/popupmenu,the name of the preference if setpref, or the name of the URL to open if you are using OpenWeb

The above settings in populate buttons give us this in game(and in the editor):

![Easy UI Builder](https://chrisashtear.github.io/docs/eui/eui3.PNG)
