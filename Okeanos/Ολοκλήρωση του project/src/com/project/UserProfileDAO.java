package com.project;
 
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.List;


public class UserProfileDAO {

     
    public List<UserProfile> list(Object NamLas, Object SelfEmail) throws SQLException {
        List<UserProfile> listProfiles = new ArrayList<>();

        try (Connection connection = DriverManager.getConnection("jdbc:postgresql://localhost:5432/postgres2","postgres", "theologis123")) {
        	if (NamLas.toString().matches("^[a-zA-Z]*$")) {
        		String sql = "SELECT * FROM profiles where (name = '"+NamLas+"' or lastname = '"+NamLas+"') and email!= '"+SelfEmail+ "'  ORDER BY name";
	            Statement statement = connection.createStatement();
	            ResultSet ps = statement.executeQuery(sql);
	            while (ps.next()) {
	            	String name = ps.getString("name");
	 			    String lastname = ps.getString("lastname");
	 			    String address = ps.getString("address");
	 			    String country = ps.getString("country");
	 			    String city = ps.getString("city");
	 			    String zipcode = ps.getString("zipcode");
	 			    String phone = ps.getString("phone");
	 			    String email = ps.getString("email");
	                UserProfile user = new UserProfile(name, lastname, address, country, city, zipcode, phone, email);
	                     
	                listProfiles.add(user);
	            }
        	}
        	else if (NamLas.toString().contentEquals("*") && SelfEmail.toString().contentEquals("admin1234")) {
	        	String sql = "SELECT * FROM profiles ORDER BY name";
	            Statement statement = connection.createStatement();
	            ResultSet ps = statement.executeQuery(sql);
	            while (ps.next()) {
	            	String name = ps.getString("name");
	 			    String lastname = ps.getString("lastname");
	 			    String address = ps.getString("address");
	 			    String country = ps.getString("country");
	 			    String city = ps.getString("city");
	 			    String zipcode = ps.getString("zipcode");
	 			    String phone = ps.getString("phone");
	 			    String email = ps.getString("email");
	                UserProfile user = new UserProfile(name, lastname, address, country, city, zipcode, phone, email);
	                     
	                listProfiles.add(user);
	            }
	        }
        } catch (SQLException ex) {
            ex.printStackTrace();
            throw ex;
        }      
         
        return listProfiles;
    }
}