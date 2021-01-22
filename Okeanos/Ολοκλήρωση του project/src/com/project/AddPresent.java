package com.project;

import java.io.*;

import javax.servlet.RequestDispatcher;
import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import java.sql.*;

@WebServlet("/AddPresent")
public class AddPresent extends HttpServlet {
	private static final long serialVersionUID = 1L;

    public AddPresent() {
        super();
        }

	protected void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		String present = request.getParameter("present");
		Integer amount =  Integer.parseInt(request.getParameter("amount"));
		 Connection c = null;
		 Statement stmt = null;
		 String selfEmail = request.getParameter("selfEmail");
		 request.setAttribute("selfEmail", selfEmail);
		 String presentemail = request.getParameter("presentemail");
	        try {
	        	Class.forName("org.postgresql.Driver");
		        c = DriverManager.getConnection("jdbc:postgresql://localhost:5432/postgres2","postgres", "theologis123");
		        
		        stmt = c.createStatement();
		        String sql=null;
		        if (presentemail!=null) {
		        	sql = "SELECT ID FROM public.Registered where email = '"+presentemail+"';";
		        }
		        else {
		        	sql = "SELECT ID FROM public.Registered where email = '"+selfEmail+"';";
		        }
		        ResultSet rs = stmt.executeQuery(sql);
		        if(rs.next()==false) {
		        	request.setAttribute("success", 0);
		        	if(selfEmail.contentEquals("admin1234")) {
	            		RequestDispatcher dispatcher = request.getRequestDispatcher("HomepageAdmin.jsp");
	                	dispatcher.forward(request, response);
	            	}
	            	else {
	            		RequestDispatcher dispatcher = request.getRequestDispatcher("Homepage.jsp");
	                	dispatcher.forward(request, response);
	            	}
		        	return;
		        }
		        Integer id = rs.getInt("id");
		        PreparedStatement ps = c.prepareStatement ("INSERT INTO public.presents(id, presentname ,amount, checked) VALUES (?, ?, ?, ?);");
		        ps.setInt(1,id);
	            ps.setString(2, present);
	            ps.setInt(3, amount);
	            ps.setInt(4, 0);
	            int i = ps.executeUpdate();
	            if(i > 0 ) {
	            	if(selfEmail.contentEquals("admin1234")) {
	            		request.setAttribute("success", 1);
	            		RequestDispatcher dispatcher = request.getRequestDispatcher("HomepageAdmin.jsp");
	                	dispatcher.forward(request, response);
	            	}
	            	else {
	            		request.setAttribute("success", 1);
	            		RequestDispatcher dispatcher = request.getRequestDispatcher("Homepage.jsp");
	                	dispatcher.forward(request, response);
	            	}
	            }
	            stmt.close();
				c.close();
	        }
	        catch(Exception se) {
	            se.printStackTrace();
	        }
		
		
	}


}
