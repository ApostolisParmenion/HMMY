package com;


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
        Integer barcode = Integer.parseInt(request.getParameter("barcode"));
        String color = request.getParameter("color");
        String description = request.getParameter("description");


        Connection c = null;
	    Statement stmt = null;
        try {
        	Class.forName("org.postgresql.Driver");
	        c = DriverManager.getConnection("jdbc:postgresql://localhost:5432/postgres","postgres", "parmen1234");
	        
	        stmt = c.createStatement();
	        String sql = "SELECT max(ID) FROM public.Products";
	        ResultSet rs = stmt.executeQuery(sql);
	        rs.next();
	        Integer maxid = rs.getInt("max");
	        stmt = c.createStatement();
	        sql = "select count(*)" + 
	        		"from public.Products U " + 
	        		"where '"+barcode+"' in (select barcode from public.Products US where U.barcode=US.barcode)";
	        rs = stmt.executeQuery(sql);
	        rs.next();
	        Integer validnum = rs.getInt("count");
	        if (validnum==0){
	            PreparedStatement ps = c.prepareStatement
	                        ("INSERT INTO public.Products( id, name, barcode, color,description) values(?,?,?,?,?)");
	            ps.setInt(1, maxid+1);
	            ps.setString(2, name);
	            ps.setInt(3, barcode);
	            ps.setString(4,color);
	            ps.setString(5, description);
	            

	  
	            int i = ps.executeUpdate();
	            
	            if(i > 0 ) {
	                response.sendRedirect("Register.jsp");
	            }
	        }
	        else {
	        	out.println("This Product's barcode Already Exists!");
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