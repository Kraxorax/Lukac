﻿using UnityEngine;
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
		public AudioClip moveSound1;				//1 of 2 Audio clips to play when player moves.
		public AudioClip moveSound2;				//2 of 2 Audio clips to play when player moves.
		public AudioClip eatSound1;					//1 of 2 Audio clips to play when player collects a food object.
		public AudioClip eatSound2;					//2 of 2 Audio clips to play when player collects a food object.
		public AudioClip drinkSound1;				//1 of 2 Audio clips to play when player collects a soda object.
		public AudioClip drinkSound2;				//2 of 2 Audio clips to play when player collects a soda object.
		public AudioClip gameOverSound;				//Audio clip to play when player dies.
		
		private Animator animator;					//Used to store a reference to the Player's animator component.
		private int food;							//Used to store player food points total during level.
		private Vector2 touchOrigin = -Vector2.one; //Used to store location of screen touch origin for mobile controls.


		private ArrayList wawesBelow;
		
		//Start overrides the Start function of MovingObject
		protected override void Start ()
		{
			//Get a component reference to the Player's animator component
			animator = GetComponent<Animator>();

			wawesBelow = new ArrayList();


			//Get the current food point total stored in GameManager.instance between levels.
			//food = GameManager.instance.playerFoodPoints;
			food = 0;
			
			//Set the foodText to reflect the current player food total.
			foodText.text = "Food: " + food;
			
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

			Vector2 odBroda = new Vector2(transform.position.x, transform.position.y) - dot;

			


			Vector2 anch = new Vector2(transform.rotation.x, transform.rotation.y); 
			float dest = Vector2.Angle(odBroda.normalized, transform.forward) * Time.deltaTime;

			float a1 = Vector2.Angle(Vector2.left, odBroda.normalized);
			float a2 = Vector2.Angle(Vector2.left, transform.forward.normalized);

			Vector2 kaM = new Vector2(transform.forward.normalized.x, transform.forward.normalized.y) - odBroda.normalized;

			float posOrNeg = isLeft (transform.position, transform.forward, odBroda) ? 1 : -1 ;


			transform.position -= new Vector3 (odBroda.normalized.x / 50, odBroda.normalized.y / 50, 0);

			transform.Rotate(Vector3.back, dest * posOrNeg);

		

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
			//Check if the tag of the trigger collided with is Exit.
			if(other.tag == "Exit")
			{
				//Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
				Invoke ("Restart", restartLevelDelay);
				
				//Disable the player object since level is over.
				enabled = false;
			}
			
			//Check if the tag of the trigger collided with is Food.
			else if(other.tag == "Food")
			{
				//Add pointsPerFood to the players current food total.
				food += pointsPerFood;
				
				//Update foodText to represent current total and notify player that they gained points
				foodText.text = "+" + pointsPerFood + " Food: " + food;
				
				//Call the RandomizeSfx function of SoundManager and pass in two eating sounds to choose between to play the eating sound effect.
				SoundManager.instance.RandomizeSfx (eatSound1, eatSound2);
				
				//Disable the food object the player collided with.
				other.gameObject.SetActive (false);
			}
			
			//Check if the tag of the trigger collided with is Soda.
			else if(other.tag == "Soda")
			{
				//Add pointsPerSoda to players food points total
				food += pointsPerSoda;
				
				//Update foodText to represent current total and notify player that they gained points
				foodText.text = "+" + pointsPerSoda + " Food: " + food;
				
				//Call the RandomizeSfx function of SoundManager and pass in two drinking sounds to choose between to play the drinking sound effect.
				SoundManager.instance.RandomizeSfx (drinkSound1, drinkSound2);
				
				//Disable the soda object the player collided with.
				other.gameObject.SetActive (false);
			}
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
				SoundManager.instance.PlaySingle (gameOverSound);
				
				//Stop the background music.
				SoundManager.instance.musicSource.Stop();
				
				//Call the GameOver function of GameManager.
				GameManager.instance.GameOver ();
			}
		}
	}
}

