using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scoreDataIOWrapper : MonoBehaviour
{
    // Start is called before the first frame update
    [System.Serializable]
	public class detailedSongInformation
	{
		public int id;
		public string composer;
		public string title;
		public int play_count;
		public int[] highscore;
		public int[] difficulty;
	}

    private detailedSongInformation result_song_input;
    private detailedSongInformation result_song_output;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // ------------------------------------------------------------
	// get details of Result
	// ------------------------------------------------------------
	void getInitialPrefData(){
        result_song_input.id = SceneController.getID() ;
        result_song_input.composer = SceneController.getComposer ();
        result_song_input.title = SceneController.getTitle ();
        // 配列で返してくれない
        //result_song_input.difficulty = SceneController.getDifficulty ();
        result_song_input.highscore = SceneController.getHighscore ();
	

    }

	// ------------------------------------------------------------
	// File I/O
	// ------------------------------------------------------------

}
