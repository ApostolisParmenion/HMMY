package com.project;

import java.sql.*;

public class Validate {

    public static boolean checkUser(String email,String password) 
    {
        boolean st =false;
        try {
        	Class.forName("org.postgresql.Driver");

            //creating connection with the database
            Connection con = DriverManager.getConnection("jdbc:postgresql://localhost:5432/postgres2","postgres", "theologis123");
            PreparedStatement ps = con.prepareStatement("select * from public.Registered where email=? and password=?");
            ps.setString(1, email);
            ps.setString(2, password);
            ResultSet rs =ps.executeQuery();
            st = rs.next();
        }
        catch(Exception e) {
            e.printStackTrace();
        }
        return st;                 
    }   
}