using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using UnityEditor.MemoryProfiler;
using UnityEngine.Video;
using TMPro;
using System;
using Unity.VisualScripting;


public class MoveCard : MonoBehaviour
{

    public static class MovieInfo
    {
        public static string movieTitle;
        public static string movieURL;
        public static int movieId;
        //public static string currentPassword;
    }

    public VideoPlayer videoPlayer;
    private string connectionString = "Server=127.0.0.1;Database=cinemadb;User ID=root;Password=1488;";
    private MySqlConnection connection;
    public GameObject[] btnCard;
    public GameObject[] btnFavorit;
    public GameObject[] btnLike;
    public GameObject[] btnGen;

    private int selectedMovieId ;
    private string titleMovie;

    private void Start()
    {
        connection = new MySqlConnection(connectionString);
        connection.Open();

        

        string sqlQuery = "SELECT cinemadb.g.namegGenres AS genre_name, m.title, m.url_move, m.movie_id " +
                              "FROM movies m " +
                              "JOIN movie_genres mg ON m.movie_id = mg.movie_id " +
                              "JOIN genres g ON mg.genre_id = g.genre_id";

        MySqlCommand cmd = new MySqlCommand(sqlQuery, connection);
        MySqlDataReader reader = cmd.ExecuteReader();

        int cardIndex = 0;
        int card2Index = 0;
        int card3Index = 0;

        while (reader.Read())
        {
            if (cardIndex < btnCard.Length)
            {
                string genreName = reader.GetString("genre_name");
                string movieTitle = reader.GetString("title");
                string movieURL = reader.GetString("url_move");
                int movieId = reader.GetInt32("movie_id");

                

                Text cardText = btnCard[cardIndex].GetComponentInChildren<Text>();
                cardText.text = movieTitle;

                Text cardText1 = btnGen[cardIndex].GetComponentInChildren<Text>();
                cardText1.text = genreName;

                Button cardButton = btnCard[cardIndex].GetComponent<Button>();

                if (cardButton != null)
                {
                    cardButton.onClick.AddListener(() => PlayMovie(movieURL, movieId, movieTitle));
                    cardButton.onClick.AddListener(() => AddToWatched(movieId, movieTitle));
                }

                cardIndex++;
            }


            //favorit
            if (card2Index < btnFavorit.Length)
            {
                string movieTitle = reader.GetString("title");
                string movieURL = reader.GetString("url_move");
                int movieId = reader.GetInt32("movie_id");
                Text cardText1 = btnFavorit[card2Index].GetComponentInChildren<Text>();


                Button cardButton = btnFavorit[card2Index].GetComponent<Button>();

                if (cardButton != null)
                {
                    cardButton.onClick.AddListener(() => AddToFavorites(movieId, movieTitle));
                }

                card2Index++;
            }
            else
            {
                Debug.LogWarning("нет объектов в инспекторе");
                break;
            }

            if (card3Index < btnLike.Length)
            {
                string movieTitle = reader.GetString("title");
                string movieURL = reader.GetString("url_move");
                int movieId = reader.GetInt32("movie_id");

                Text cardText2 = btnLike[card3Index].GetComponentInChildren<Text>();


                Button cardButton = btnLike[card3Index].GetComponent<Button>();

                if (cardButton != null)
                {
                    cardButton.onClick.AddListener(() => AddToLike(movieId, movieTitle));
                }

                card3Index++;
            }
            else
            {
                Debug.LogWarning("нет объектов в инспекторе");
                break;
            }



        }
        reader.Close();
        connection.Close();
    }
    

    private void PlayMovie(string movieURL, int movieId, string movieTitle)
    {
        videoPlayer.url = movieURL;
        selectedMovieId = movieId;
        videoPlayer.Play();
    }

    public void AddToFavorites(int movieId, string movieTitle)
    {      
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        string query = $"INSERT INTO favourites (movie_id, user_id, title) VALUES (@movieId, @userId, @title)";
        MySqlCommand command = new MySqlCommand(query, connection);

        command.Parameters.AddWithValue("@movieId", movieId);
        command.Parameters.AddWithValue("@userId", UserInfo.user_id);
        command.Parameters.AddWithValue("@title", movieTitle);

        try
        {
            command.ExecuteNonQuery();
            Debug.Log("Фильм добавлен в избранное.");
        }
        catch (Exception ex)
        {
            Debug.LogError("Произошла ошибка при добавлении фильма в избранное: " + ex);
        }
    }

    public void AddToLike(int movieId, string movieTitle)
    {
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        string query = $"INSERT INTO `like` (movie_id, user_id, title) VALUES (@movieId, @userId, @title)";
        MySqlCommand command = new MySqlCommand(query, connection);

        command.Parameters.AddWithValue("@movieId", movieId);
        command.Parameters.AddWithValue("@userId", UserInfo.user_id);
        command.Parameters.AddWithValue("@title", movieTitle);

        try
        {
            command.ExecuteNonQuery();
            Debug.Log("Фильм добавлен в like.");
        }
        catch (Exception ex)
        {
            Debug.LogError("Произошла ошибка при добавлении фильма в like: " + ex);
        }
    }

    public void AddToWatched(int movieId, string movieTitle)
    {


        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        string query = $"INSERT INTO `watched_movies` (movie_id, user_id, title) VALUES (@movieId, @userId, @title)";
        MySqlCommand command = new MySqlCommand(query, connection);

        command.Parameters.AddWithValue("@movieId", movieId);
        command.Parameters.AddWithValue("@userId", UserInfo.user_id);
        command.Parameters.AddWithValue("@title", movieTitle);

        try
        {
            command.ExecuteNonQuery();
            
        }
        catch (Exception ex)
        {
            Debug.LogError("ошибка при добавлении фильма в History: " + ex);
        }
    }


}