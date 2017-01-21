using UnityEngine;
using System.Collections;
using UnityEngine.UI;	//Allows us to use UI.

namespace Completed
{
	//Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
	public class Player : MovingObject
	{
		public float restartLevelDelay = 1f;		//Delay time in seconds to restart level.
		public int pointsPerFood = 10;				//Number of points to add to player food points when picking up a food object.
		public int pointsPerSoda = 20;				//Number of points to add to player food points when picking up a soda object.
		public int wallDamage = 1;					//How much damage a player does to a wall when chopping it.
		public Text foodText;						//UI Text to display current player food total.
		private Animator animator;					//Used to store a reference to the Player's animator component.
		private int food;							//Used to store player food points total during level.
		private Vector2 touchOrigin = -Vector2.one; //Used to store location of screen touch origin for mobile controls.


		private ArrayList wawesBelow;
		
		//Start overrides the Start function of MovingObject
		protected override void Start ()
		{
			//Get a component reference to the Player's animator component
			animator = GetComponent<Animator>();
			rb = GetComponent<Rigidbody>();

			wawesBelow = new ArrayList();


			//Get the current food point total stored in GameManager.instance between levels.
			//food = GameManager.instance.playerFoodPoints;
			food = 0;
			
			//Set the foodText to reflect the current player food total.

			//Call the Start function of the MovingObject base class.
			base.Start ();
		}
		
		
		Vector2 getForce()
		{
			BackgroundParallax bp = FindObjectOfType<BackgroundParallax>();
			Vector2 fv = new Vector2(0, 0);
			float strength = 0;

			Wawe[] ws = FindObjectsOfType<Wawe>();

//			Debug.Log("wawes below " + wawesBelow.Count);

			foreach (Wawe w in wawesBelow)
			{
				fv += w.getVector();
			}
			
			return fv.normalized;
		}

		void OnCollisionEnter(Collision coll)
		{
//			Debug.Log("COLL EEE " + coll.ToString());

			wawesBelow.Add(coll.gameObject.GetComponent<Wawe>());
		}

		void OnCollisionExit(Collision coll)
		{
//			Debug.Log("COLL EX " + coll.ToString());

			wawesBelow.Remove(coll.gameObject.GetComponent<Wawe>());
		}

		void OnCollisionStay(Collision coll)
		{
			//Debug.Log("COLL SSS " + coll.ToString());
			//transform.position += coll.other.gameObject
		}


		private void Update ()
		{
			Vector2 f = getForce();
			food = (int)f.magnitude;
			Vector3 f3 = new Vector3(f.x, f.y, 0);
			transform.position += f3 / 100;
			transform.Rotate(f3/30);

			

			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); 
			Vector2 dot = new Vector2(ray.origin.x, ray.origin.y);

			Vector2 odBroda = dot - new Vector2(transform.position.x, transform.position.y) ;

			Vector2 anch = new Vector2(transform.rotation.x, transform.rotation.y); 
			float dest = Vector2.Angle(transform.up.normalized, odBroda.normalized);

//			float a1 = Vector2.Angle(Vector2.left, odBroda.normalized);
//			float a2 = Vector2.Angle(Vector2.left, transform.forward.normalized);

			Vector2 kaM = new Vector2(transform.up.normalized.x, transform.up.normalized.y) - odBroda.normalized;

			bool lor = isLeft (transform.position, transform.up, odBroda);

			transform.position += new Vector3 (odBroda.normalized.x / 50, odBroda.normalized.y / 50, 0);
//			Debug.Log ("dest " + dest);
			if (dest < 10.0f) { 
//				Debug.Log ("To je mali ugao"); 
			} else if (dest > 170.0f) {
//				Debug.Log ("OVo je preveliki ugao"); 

			} else {
				float posOrNeg = 1;
				if (lor && dest > 89.9f) { 
					Debug.Log ("to the left, to the left");
					posOrNeg = -1;
				} else {
//					Debug.Log ("dont be so lazy");
				}
				float smooth = 2.0f;
				Vector3 eu = transform.rotation.eulerAngles;
//				Debug.Log (transform.up);
				Quaternion target = Quaternion.Euler (eu.x, 0, eu.z + 20 * posOrNeg); 
				transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
			
			}

//			transform.LookAt (new Vector3(odBroda.x, odBroda.y, transform.position.z) + transform.position);
		
		}	
		public Vector3 eulerAngleVelocity;
		public Rigidbody rb;

		public float angleOfAngle(float ang) {
			return ang;

		}

		public bool isLeft(Vector2 a, Vector2 b, Vector2 c){
			return ((b.x - a.x)*(c.y - a.y) - (b.y - a.y)*(c.x - a.x)) > 0;
		}
		//AttemptMove overrides the AttemptMove function in the base class MovingObject
		//AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
		//protected override void AttemptMove <T> (int xDir, int yDir)
		//{
		//	//Every time player moves, subtract from food points total.
		//	//food--;
			
		//	//Update food text display to reflect current score.
		//	foodText.text = "Food: " + food;
			
		//	//Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
		//	base.AttemptMove <T> (xDir, yDir);
			
		//	//Hit allows us to reference the result of the Linecast done in Move.
		//	RaycastHit2D hit;
			
		//	//If Move returns true, meaning Player was able to move into an empty space.
		//	if (Move (xDir, yDir, out hit)) 
		//	{
		//		//Call RandomizeSfx of SoundManager to play the move sound, passing in two audio clips to choose from.
		//		SoundManager.instance.RandomizeSfx (moveSound1, moveSound2);
		//	}
			
		//	//Since the player has moved and lost food points, check if the game has ended.
		//	CheckIfGameOver ();
			
		//	//Set the playersTurn boolean of GameManager to false now that players turn is over.
		//	GameManager.instance.playersTurn = false;
		//}
		
		
		//OnCantMove overrides the abstract function OnCantMove in MovingObject.
		//It takes a generic parameter T which in the case of Player is a Wall which the player can attack and destroy.
		protected override void OnCantMove <T> (T component)
		{
			//Set hitWall to equal the component passed in as a parameter.
			Wall hitWall = component as Wall;
			
			//Call the DamageWall function of the Wall we are hitting.
			hitWall.DamageWall (wallDamage);
			
			//Set the attack trigger of the player's animation controller in order to play the player's attack animation.
			animator.SetTrigger ("playerChop");
		}
		
		
		//OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
		private void OnTriggerEnter2D (Collider2D other)
		{
		}
		
		
		//Restart reloads the scene when called.
		private void Restart ()
		{
			//Load the last scene loaded, in this case Main, the only scene in the game.
			Application.LoadLevel (Application.loadedLevel);
		}
		
		
		//LoseFood is called when an enemy attacks the player.
		//It takes a parameter loss which specifies how many points to lose.
		public void LoseFood (int loss)
		{
			//Set the trigger for the player animator to transition to the playerHit animation.
			animator.SetTrigger ("playerHit");
			
			//Subtract lost food points from the players total.
			food -= loss;
			
			//Update the food display with the new total.
			foodText.text = "-"+ loss + " Food: " + food;
			
			//Check to see if game has ended.
			CheckIfGameOver ();
		}
		
		
		//CheckIfGameOver checks if the player is out of food points and if so, ends the game.
		private void CheckIfGameOver ()
		{
			//Check if food point total is less than or equal to zero.
			if (food <= 0) 
			{
				//Call the PlaySingle function of SoundManager and pass it the gameOverSound as the audio clip to play.
//				SoundManager.instance.PlaySingle (gameOverSound);
//				
//				//Stop the background music.
//				SoundManager.instance.musicSource.Stop();
//				
//				//Call the GameOver function of GameManager.
//				GameManager.instance.GameOver ();
			}
		}
	}
}

