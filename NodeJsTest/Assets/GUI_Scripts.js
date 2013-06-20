private var textFieldString : String = "Socket Testing String";
private var myTCP; 
private var time : int;
private var cube : GameObject;
private var x : int;
private var y : int;
private var z : int;

function Awake() {
	myTCP = gameObject.AddComponent(s_TCP);
	
	var projectile : Rigidbody;
    InvokeRepeating("LaunchProjectile", 1, 0.4);
}

function Update(){
	cube = GameObject.Find('Cube');
    cube.transform.position = Vector3(x, y, z);
}

function Start(){
	
}

function LaunchProjectile () {
    myTCP.writeSocket("get_status");
    textFieldString = myTCP.readSocket();
    
    var a : String[] = textFieldString.Split(","[0]);
    
    x = parseFloat(a[0]);
    y = parseFloat(a[1]);
    z = parseFloat(a[2]);
    
    //textFiledString.split(',');
    
}
    
    
function OnGUI () {
 if(myTCP.socketReady == false) {
  if (GUI.Button (Rect (20,10,80,20),"Connect")) {
   myTCP.setupSocket();
  }
 }
 else {
  myTCP.maintainConnection();

  if (GUI.Button (Rect (20,40,80,20), "Level 1")) {
   myTCP.writeSocket(" The is from Level 1 Button");
   
  }
  if (GUI.Button (Rect (20,70,80,20), "Level 2")) {
    
  }
  
  textFieldString = GUI.TextField (Rect (25, 100, 300, 30), textFieldString);
  
  if (GUI.Button (Rect (20,140,80,20),"Disconnect")) {
   myTCP.closeSocket();
   textFieldString = "Socket Disconnected...";
  }
 }
}