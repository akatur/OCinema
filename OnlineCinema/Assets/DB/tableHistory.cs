using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.UI;

public class tableHistory : MonoBehaviour
{

    private string connectionString = "Server=127.0.0.1;Database=cinemadb;User ID=root;Password=1488;";

    public GameObject[] btnGen;

    private MySqlConnection connection;
    private MySqlDataReader reader;

    public Text textCanvas;


    void Start()
    {
        connection = new MySqlConnection(connectionString);
        connection.Open();



        string sqlQuery = "SELECT watched_id,title from watched_movies ";


        MySqlCommand cmd = new MySqlCommand(sqlQuery, connection);
        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            textCanvas.text += "\n                    " + reader.GetString(0) + "                       " + reader.GetString(1);
        }
        reader.Close();
    }

    public void OnButtonClick()
    {


        if (connection == null || connection.State != ConnectionState.Open)
        {
            Debug.LogError("The database connection is not established or closed.");
            return;
        }

        string sqlQuery = "SELECT watched_id, title from watched_movies";

        using (MySqlCommand cmd = new MySqlCommand(sqlQuery, connection))
        {
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    textCanvas.text += "\n" + reader.GetString(0) + " " + reader.GetString(1);
                }
            }
        }
    }

    public void OnButtonClickDeliteHis()
    {

        string deleteQuery = "DELETE FROM watched_movies";

        using (MySqlCommand cmd = new MySqlCommand(deleteQuery, connection))
        {
            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                textCanvas.text = ""; 
            }
            else
            {
                Debug.LogWarning("History was not cleared. It might have been empty already.");
            }
        }


    }
}