using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient;
using UnityEngine.UI;
using System;
using System.Data;
using UnityEngine.Video;
using UnityEditor.MemoryProfiler;
using static DBmanager;
using UnityEditor.Search;

public static class ConnectionInfo
{
    public static string ip = "127.0.0.1";
    public static string uid = "root";
    public static string pwd = "1488";
    public static string database = "cinemadb";
}

public static class UserInfo
{
    public static string currentName;
    public static string currentLogin;
    public static string user_id;
    public static string currentPassword;
}

public class DBmanager : MonoBehaviour
{
    public GameObject CanvasRegLog;
    public GameObject canvasMenuUI;

    private movePlayer movePlayerComponent;
    private menuUiControll menuUi;
    private videocontroll videoCon;

    movePlayer movePlayer;

    void Start()
    {
        
        GameObject camObject = GameObject.Find("CamPos");
        movePlayerComponent = camObject.GetComponent<movePlayer>();
        movePlayerComponent.enabled = false;


        GameObject uiObject = GameObject.Find("CanvasMenuUI");
        menuUi = uiObject.GetComponent<menuUiControll>();
        menuUi.enabled = false;
        

        GameObject videoConP = GameObject.Find("VideoPlayer");
        videoCon = videoConP.GetComponent<videocontroll>();
        videoCon.enabled = false;

        canvasMenuUI.SetActive(false);




    }


    static string conectionString = $"server={ConnectionInfo.ip}; " +
        $"uid={ConnectionInfo.uid}; " +
        $"pwd={ConnectionInfo.pwd}; " +
        $"Database = {ConnectionInfo.database}; " +
        $"SSLMode=none";

    //инпуты аунтефикации
    public InputField InputLogAut;
    public InputField InputPwdAut;
    public InputField InputNicknameLog;
     InputField logButton;

    static MySqlConnection con;

    private void Awake()
    {
        con = new MySqlConnection(conectionString);
        try
        {
            con.Open();

            AddMovie();

        }
        catch (Exception ex)
        {
            Debug.Log("Произошла ошибка!" + ex);
        }
    }


    public InputField InputNameMovie;
    public InputField InputNameGenre;
    public InputField InputUrlMovie;
    
    private void AddMovie( )
    {
        string query = "Insert Into movies (movie_id,title,url_move) VALUES (@movieId, @title, @url_move)";

        string namemove = InputNameMovie.text.Trim();
        string nameGenre = InputNameGenre.text.Trim();
        string urlMovie = InputUrlMovie.text.Trim();
        
        if (namemove == "" || nameGenre == "" || urlMovie == "" || InputNicknameReg.text.Trim() == "")
        {
            Debug.Log("Введите данные о фильме");
        }
        else
        {
            if (GetRegUser())
            {
                Debug.Log("Данные приняты");

                CanvasRegLog.SetActive(true);
                movePlayerComponent.enabled = true;
                menuUi.enabled = true;
                videoCon.enabled = true;
                canvasMenuUI.SetActive(true);

            }
            else
            {
                Debug.Log("Чтото не так");
            }
        }
    }

    public bool GetAddMovie()
    {
        string query = $"Select * From users where login = '{InputLogReg.text.Trim()}'";
        if (isAny(query))
        {
            Debug.Log("нет доступа");
            return false;
        }
        else
        {
            try
            {
                query = $"Insert Into users (login,password,username) " +
               $"Values ('{InputLogReg.text.Trim()}'," +
               $"'{InputPwdReg.text.Trim()}'," +
               $"'{InputNicknameReg.text.Trim()}')";
                var command = new MySqlCommand(query, con);
                command.ExecuteNonQuery();
                UserInfo.user_id = command.LastInsertedId.ToString();
                command.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
                return false;
            }
        }
    }





    private void OnApplicationQuit()
    {
        con.Close();
    }



    public void LoginUser()
    {
        string login = InputLogAut.text.Trim();
        string password = InputPwdAut.text.Trim();
        if (login == "" || password == "")
        {
            Debug.Log("Введите логин или пароль");
        }
        else
        {
            if (LoginUser(login, password))
            {
                UserInfo.currentLogin = login;
                Debug.Log("Данные приняты");
                CanvasRegLog.SetActive(false);
                movePlayerComponent.enabled = true;
                menuUi.enabled = true;
                videoCon.enabled = true;
                canvasMenuUI.SetActive(true);
            }
            else
            {
                Debug.Log("eror");
            }
        }
    }
    
    public bool LoginUser(string login, string password)
    {
        string query = "SELECT user_id, password FROM users WHERE login = @login";
        using (var cmd = new MySqlCommand(query, con))
        {
            cmd.Parameters.AddWithValue("@login", login);

            try
            {
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                       
                        string storedPassword = reader.GetString("password");
                        if (password == storedPassword)
                        {
                            UserInfo.user_id = reader.GetInt32("user_id").ToString();

                           
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }
        return false;
    }


    public static bool isAny(string query)
    {
        var cmd = new MySqlCommand(query, con);
        try
        {
            var reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                cmd.Dispose();
                return true;
            }
            else
            {
                cmd.Dispose();
                return false;
            }
        }
        catch (Exception)
        {
            cmd.Dispose();
            return false;
        }
    }


    //инпуты регестрации
    public InputField InputLogReg;
    public InputField InputPwdReg;
    public InputField InputNicknameReg;
     InputField regButton;



    public void RegUser()
    {
        string login = InputLogReg.text.Trim();
        string password = InputPwdReg.text.Trim();
        if (login == "" || password == "" || InputNicknameReg.text.Trim() == "")
        {
            Debug.Log("Введите логин или пароль");
        }
        else
        {
            if (GetRegUser())
            {
                Debug.Log("Данные приняты");

                CanvasRegLog.SetActive(true);
                movePlayerComponent.enabled = true;
                menuUi.enabled = true;
                videoCon.enabled = true;
                canvasMenuUI.SetActive(true);
               
            }
            else
            {
                Debug.Log("Чтото не так");
            }
        }
    }

        public bool GetRegUser()
        {
            string query = $"Select * From users where login = '{InputLogReg.text.Trim()}'";
            if (isAny(query))
            {
                Debug.Log("нет доступа");
                return false;
            }
            else
            {
                try
                {
                    query = $"Insert Into users (login,password,username) " +
                   $"Values ('{InputLogReg.text.Trim()}'," +
                   $"'{InputPwdReg.text.Trim()}'," +
                   $"'{InputNicknameReg.text.Trim()}')";
                    var command = new MySqlCommand(query, con);
                    command.ExecuteNonQuery();
                    UserInfo.user_id = command.LastInsertedId.ToString();
                    command.Dispose();
                    return true;
                }
                catch (Exception ex)
                {
                    Debug.Log(ex);
                    return false;
                }
            }
        }

    


    }


    


   