/* 	
============================================================================================================
	 _	 _  ____  _      _  ____  _____  _____  _______  ____  _____
	| | | || ___|| |    | || ___||  _  || ___ ||__   __|| ___|| ___ |
	| |_| || |__ | |    | || |   | | | || |_| |   | |   | |__ | |_| |
	|  _  || ___|| |    | || |   | | | || ____|   | |   | ___|| __  |
	| | | || |__ | |___ | || |__ | |_| || |       | |   | |__ | | \ \
	|_| |_||____||_____||_||____||_____||_|       |_|   |____||_|  \_\
	
		 _______  _   _  _______  _____  _____   _  _____  _
		|__   __|| | | ||__   __||  _  || ___ | | ||  _  || |
		   | |   | | | |   | |   | | | || |_| | | || |_| || |
		   | |   | | | |   | |   | | | ||  _  | | ||  _  || |
		   | |   | |_| |   | |   | |_| || | \ \ | || | | || |___
		   |_|   |_____|   |_|   |_____||_|  \_\|_||_| |_||_____|
	   
				   	 _________________________
					|_____by: Andrew Gotow____|
			
============================================================================================================
	
	This is an incredibly simple script that makes the light it is attached to slowly pulse, simply by setting
	it's brightness to a sine wave, with the parameter being the total time the game has been running.	
*/

function Update () {
	light.intensity = Mathf.Sin( Time.time );
}