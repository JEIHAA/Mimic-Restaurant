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
    [Header("�����ͺ��̽� �ּ�(For Example: www.example.com")]
    [SerializeField, TextArea] private string dburl = string.Empty;
    [Header("�����ͺ��̽� �̸�")]
    [SerializeField] private string dbname = string.Empty;
    [Header("�����ͺ��̽� ���̵�")]
    [SerializeField] private string dbid = string.Empty;
    [Header("�����ͺ��̽� ��й�ȣ")]
    [SerializeField] private string dbpw = string.Empty;

    private string connectionString = string.Empty;
    private string sql = string.Empty;
    private MySqlConnection conn = null;
    
    public static DBManager instance = null;  //Singleton 
    
    private void Awake()
    {
        instance = this; 
    }

    #region["�����ͺ��̽� ����"] 
    public void ConnectDB()
    {
        connectionString = string.Format("Server={0};Database={1};User ID={2};Password={3};SslMode=none;", dburl, dbname, dbid, dbpw);
        conn = new MySqlConnection(connectionString);
        conn.Open(); 
    }
    #endregion

    #region["�����ͺ��̽� ���� ����"] 
    public void DisconnectDB()
    {
        if(conn != null)
        {
            conn.Close();
            conn = null; 
        }
    }
    #endregion

    #region["ȸ������ ����: ȸ������ ����"] 
    public int InsertMember(MemberDTO memberdto)
    {
        try
        {
            ConnectDB();
            sql = "insert into users values (@id, @password, @nickname)";
            MySqlCommand icmd = new MySqlCommand(sql, conn);
            //SQL Injection ���� 
            icmd.Parameters.Add(new MySqlParameter("id", memberdto.id));
            //BCrypt�� �̿��� ��й�ȣ ��ȣȭ 
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

    #region["���̵� �ߺ�üũ"]
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
                    //�ߺ� 
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

    #region["�α���"] 
    public int Login(string _id, string _pw)
    {
        try
        {
            //0: ���� �� ���α׷� ����, 1: �α��� ����, 2: �α��� ���� 
            int result = 0;
            ConnectDB();
            sql = "select password from users where id = @id";
            MySqlCommand scmd = new MySqlCommand(sql, conn);
            scmd.Parameters.Add(new MySqlParameter("id", _id));
            MySqlDataReader mdr = scmd.ExecuteReader();
            string password_db = string.Empty;
            if (mdr.Read())
            {
                //��ȣȭ�� ��й�ȣ �������� 
                if (!string.IsNullOrEmpty(mdr["password"].ToString()))
                {
                    password_db = mdr["password"].ToString();
                }
            }
            //BCrypt �˰����� �̿��� ��й�ȣ �˻� 
            if(!string.IsNullOrEmpty(password_db))
            {
                if (BCrypt.Net.BCrypt.Verify(_pw, password_db))
                {
                    //�α��� ���� 
                    result = 1;
                }
                else
                {
                    //�α��� ���� (��й�ȣ�� �ٸ�) 
                    result = 2;
                }
            }
            else
            {
                //�α��� ����(�����ͺ��̽��� ����� ��й�ȣ�� NULL => ���̵� �������� ����) 
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
