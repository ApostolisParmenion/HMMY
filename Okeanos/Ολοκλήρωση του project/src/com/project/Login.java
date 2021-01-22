package com.project;

import java.io.*;
import javax.servlet.*;
import javax.servlet.http.*;

public class Login extends HttpServlet {
	private static final long serialVersionUID = 1L;

    public Login() {
        super();

    }
 
    protected void doPost(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {
        response.setContentType("text/html;charset=UTF-8");
        PrintWriter out = response.getWriter();
        
        String email = request.getParameter("email");
        String password = request.getParameter("password");
        if (email.contentEquals("admin1234") && password.contentEquals("admin1234")) {
        	request.setAttribute("selfEmail", email);
        	RequestDispatcher rs = request.getRequestDispatcher("HomepageAdmin.jsp");
        	rs.include(request, response);
        }
        else if (Validate.checkUser(email, password))
        {
        	request.setAttribute("selfEmail", email);
        	RequestDispatcher rs = request.getRequestDispatcher("Homepage.jsp");
            rs.include(request, response);
        }
        else
        {
           out.println("Username or Password incorrect");
           RequestDispatcher rs = request.getRequestDispatcher("Index.jsp");
           rs.include(request, response);
        }
    }  
}