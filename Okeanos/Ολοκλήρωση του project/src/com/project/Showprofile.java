package com.project;



import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.List;
import java.io.*;

import javax.servlet.RequestDispatcher;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;


public class Showprofile extends HttpServlet {
	private static final long serialVersionUID = 1L;

    public Showprofile() {
        super();

    }
 
    protected void doGet(HttpServletRequest request, HttpServletResponse response)
    throws ServletException, IOException {
	   	response.setContentType("text/html;charset=UTF-8");
	   	Connection c = null;
	   	String selfEmail = request.getParameter("selfEmail");

	    String UseEmail = selfEmail;
	    
	    Object delete = request.getParameter("delete");
	    Object presentname = request.getParameter("presentname");

	    Object select = request.getParameter("select");
	    int addamount;
	    try {
	    	addamount = Integer.parseInt( request.getParameter("addAmount"));
	    }
	    catch(Exception e) {
	    	addamount=0;
	    }
	    boolean myemail=true;

	    String Emaill = request.getParameter("email");
	    System.out.println(selfEmail);
	    System.out.println(Emaill);

	    if(Emaill!=null) {
		    if (!Emaill.contentEquals(selfEmail)) {
		    	UseEmail = Emaill;
		    	myemail=false;
		    }
	    }
	    request.setAttribute("myemail", myemail);
	    Statement stmt = null;
	    try {
	    	Class.forName("org.postgresql.Driver");
			c = DriverManager.getConnection("jdbc:postgresql://localhost:5432/postgres2","postgres", "theologis123");
			stmt = c.createStatement();
			String sql = "SELECT *"+
			          "FROM public.profiles where email = '"+ UseEmail +"'";
			ResultSet ps = stmt.executeQuery(sql);
			while (ps.next()) {
			    String name = ps.getString("name");
			    String lastname = ps.getString("lastname");
			    String address = ps.getString("address");
			    String country = ps.getString("country");
			    String city = ps.getString("city");
			    String zipcode = ps.getString("zipcode");
			    String phone = ps.getString("phone");
			    String email = ps.getString("email");
			    request.setAttribute("name", name);	
			    request.setAttribute("lastname", lastname);	
			    request.setAttribute("address", address);
			    request.setAttribute("city", city);	
			    request.setAttribute("country", country);
			    request.setAttribute("zipcode", zipcode);	
			    request.setAttribute("phone", phone);	
			    request.setAttribute("email", email);
			}

			stmt = c.createStatement();
			sql = "SELECT id FROM public.registered where email = '"+ UseEmail +"'";
			ps = stmt.executeQuery(sql);
			ps.next();
			Integer id =  ps.getInt("id");
			if (delete!=null) {
				PreparedStatement rs = c.prepareStatement("DELETE FROM public.presents WHERE presentname = '" + presentname+ "' and id = '"+id +"';");
	        	rs.executeUpdate();
			}
			if (addamount!=0) {
				stmt = c.createStatement();
				sql = "SELECT amount FROM public.presents WHERE presentname = '" + presentname+ "' and id = '"+id +"'";
				ps = stmt.executeQuery(sql);
				int tempAmount=0;
				while (ps.next()) {
					tempAmount=ps.getInt("amount");
				}
				int finalAmount=tempAmount-addamount;
				if(finalAmount<=0) {
					select=true;
					finalAmount=0;
				}
				PreparedStatement rs = c.prepareStatement("UPDATE public.presents SET amount='"+(finalAmount)+"' WHERE presentname = '" + presentname+ "' and id = '"+id +"';");
	        	rs.executeUpdate();
	        	request.setAttribute("presentname", presentname);
			}
			if (select!=null) {
				PreparedStatement rs = c.prepareStatement("UPDATE public.presents SET checked='"+1+"' WHERE presentname = '" + presentname+ "' and id = '"+id +"';");
	        	rs.executeUpdate();
	        	request.setAttribute("presentname", presentname);
			}
			
			
			stmt = c.createStatement();
			
			sql = "SELECT * FROM public.presents where id = "+ id;
			ps = stmt.executeQuery(sql);
			List<String> listPresents = new ArrayList<>();
			List<Integer> listChecked = new ArrayList<>();
			List<Integer> listAmounts = new ArrayList<>();
			while (ps.next()) {
				String present = ps.getString("presentname");
				Integer check = ps.getInt("checked");
				Integer amount = ps.getInt("amount");
				listPresents.add(present);
				listAmounts.add(amount);
				listChecked.add(check);
			}
			request.setAttribute("listPresents", listPresents);
			request.setAttribute("listAmounts", listAmounts);
			request.setAttribute("listChecked", listChecked);
			request.setAttribute("selfEmail", selfEmail);
			stmt.close();
			c.close();
			RequestDispatcher rs = request.getRequestDispatcher("Profile.jsp");
            rs.include(request, response);
		}
		catch ( Exception e ) {
		   System.err.println( e.getClass().getName()+": "+ e.getMessage() );
		}		    
	}
}
