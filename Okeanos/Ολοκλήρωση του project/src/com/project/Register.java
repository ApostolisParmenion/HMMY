package com.project;


import java.io.*;
import javax.servlet.*;
import javax.servlet.http.*;
import java.sql.*;

public class Register extends HttpServlet {
	private static final long serialVersionUID = 1L;

    public Register() {
        super();

    }
    
    protected void doGet(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {

    	response.setContentType("text/html;charset=UTF-8");
        PrintWriter out = response.getWriter();
        String name = request.getParameter("name");
        String email = request.getParameter("email");
        String pass = request.getParameter("password");
        String lastname = request.getParameter("lastname");
        String country = request.getParameter("country");
        String city = request.getParameter("city");
        String address = request.getParameter("address");
        String zipcode = request.getParameter("zipcode");
        String phonenumber = request.getParameter("phonenumber");


        Connection c = null;
	    Statement stmt = null;
        try {
        	Class.forName("org.postgresql.Driver");
	        c = DriverManager.getConnection("jdbc:postgresql://localhost:5432/postgres2","postgres", "theologis123");
	        
	        stmt = c.createStatement();
	        String sql = "SELECT max(ID) FROM public.Registered";
	        ResultSet rs = stmt.executeQuery(sql);
	        rs.next();
	        Integer maxid = rs.getInt("max");
	        stmt = c.createStatement();
	        sql = "select count(*)" + 
	        		"from public.Registered U " + 
	        		"where '"+email+"' in (select email from public.Registered US where U.id=US.ID)";
	        rs = stmt.executeQuery(sql);
	        rs.next();
	        Integer validnum = rs.getInt("count");
	        if (validnum==0){
	            PreparedStatement ps = c.prepareStatement
	                        ("INSERT INTO public.Registered( id, Email, Password) values(?,?,?)");
	            PreparedStatement ms = c.prepareStatement
                        ("INSERT INTO public.profiles(id, name,lastname,email, address, city, zipcode, phone, country) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?);");
	            ps.setInt(1, maxid+1);
	            ps.setString(2, email);
	            ps.setString(3, pass);
	            
	            ms.setInt(1, maxid+1);
	            ms.setString(2, name);
	            ms.setString(3, lastname);
	            ms.setString(4, email);
	            ms.setString(5, address);
	            ms.setString(6, city);
	            ms.setString(7, zipcode);
	            ms.setString(8, phonenumber);
	            ms.setString(9, country);
	            
	            int i = ps.executeUpdate();
	            int j = ms.executeUpdate();
	            
	            if(i > 0 & j>0) {
	                response.sendRedirect("Register.jsp");
	            }
	        }
	        else {
	        	out.println("This Email Already Exists!");
	        	RequestDispatcher ns = request.getRequestDispatcher("Index.jsp");
	            ns.include(request, response);
	        }
	        stmt.close();
			c.close();
        
        }
        catch(Exception se) {
            se.printStackTrace();
        }
	
    }
}