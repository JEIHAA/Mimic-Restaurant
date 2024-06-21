using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.UI;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class DBManager : MonoBehaviour
{
    [Header("데이터베이스 주소(For Example: www.example.com")]
    [SerializeField, TextArea] private string dburl = string.Empty;
    [Header("데이터베이스 이름")]
    [SerializeField] private string dbname = string.Empty;
    [Header("데이터베이스 아이디")]
    [SerializeField] private string dbid = string.Empty;
    [Header("데이터베이스 비밀번호")]
    [SerializeField] private string dbpw = string.Empty;

    private string connectionString = string.Empty;
    private string sql = string.Empty;
    private MySqlConnection conn = null;
    
    public static DBManager instance = null;  //Singleton 
    
    private void Awake()
    {
        instance = this; 
    }

    #region["데이터베이스 연결"] 
    public void ConnectDB()
    {
        connectionString = string.Format("Server={0};Database={1};User ID={2};Password={3};SslMode=none;", dburl, dbname, dbid, dbpw);
        conn = new MySqlConnection(connectionString);
        conn.Open(); 
    }
    #endregion

    #region["데이터베이스 연결 종료"] 
    public void DisconnectDB()
    {
        if(conn != null)
        {
            conn.Close();
            conn = null; 
        }
    }
    #endregion

    #region["회원정보 삽입: 회원가입 전용"] 
    public int InsertMember(MemberDTO memberdto)
    {
        try
        {
            ConnectDB();
            sql = "insert into users values (@id, @password, @nickname)";
            MySqlCommand icmd = new MySqlCommand(sql, conn);
            //SQL Injection 방지 
            icmd.Parameters.Add(new MySqlParameter("id", memberdto.id));
            //BCrypt를 이용한 비밀번호 암호화 
            string password_hash = BCrypt.Net.BCrypt.HashPassword(memberdto.password);
            icmd.Parameters.Add(new MySqlParameter("password", password_hash));
            icmd.Parameters.Add(new MySqlParameter("nickname", memberdto.nickname));
            int result = icmd.ExecuteNonQuery();
            icmd.Dispose();
            DisconnectDB();
            return result;
        }
        catch(Exception ex)
        {
            Debug.LogError("Error: " + ex.Message); 
            return 0; 
        }
   
    }
    #endregion

    #region["아이디 중복체크"]
    public int IDOverlapCheck(string _id)
    {
        try
        {
            int result = 0;
            ConnectDB();
            sql = "select id from users where id = @id";
            MySqlCommand scmd = new MySqlCommand(sql, conn);
            scmd.Parameters.Add(new MySqlParameter("id", _id));
            MySqlDataReader mdr = scmd.ExecuteReader();
            if (mdr.Read())
            {
                if (!string.IsNullOrEmpty(mdr["id"].ToString()))
                {
                    //중복 
                    result = 1;
                }
                else
                {
                    result = 0;
                }
            }
            mdr.Close();
            DisconnectDB();
            return result;
        }
        catch(Exception ex)
        {
            Debug.LogError("Error: " + ex.Message);
            return 0; 
        }
    }
    #endregion

    #region["로그인"] 
    public int Login(string _id, string _pw)
    {
        try
        {
            //0: 서버 및 프로그램 오류, 1: 로그인 성공, 2: 로그인 실패 
            int result = 0;
            ConnectDB();
            sql = "select password from users where id = @id";
            MySqlCommand scmd = new MySqlCommand(sql, conn);
            scmd.Parameters.Add(new MySqlParameter("id", _id));
            MySqlDataReader mdr = scmd.ExecuteReader();
            string password_db = string.Empty;
            if (mdr.Read())
            {
                //암호화된 비밀번호 가져오기 
                if (!string.IsNullOrEmpty(mdr["password"].ToString()))
                {
                    password_db = mdr["password"].ToString();
                }
            }
            //BCrypt 알고리즘을 이용한 비밀번호 검사 
            if(!string.IsNullOrEmpty(password_db))
            {
                if (BCrypt.Net.BCrypt.Verify(_pw, password_db))
                {
                    //로그인 성공 
                    result = 1;
                }
                else
                {
                    //로그인 실패 (비밀번호가 다름) 
                    result = 2;
                }
            }
            else
            {
                //로그인 실패(데이터베이스에 저장된 비밀번호가 NULL => 아이디가 존재하지 않음) 
                result = 2; 
            }
            mdr.Close();
            DisconnectDB();
            return result;
        }
        catch(Exception ex)
        {
            Debug.LogError("Error: " + ex.Message);
            return 0; 
        }
    }
    #endregion 

}
