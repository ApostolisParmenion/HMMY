package com.project;


import java.io.IOException;
import java.sql.SQLException;
import java.util.List;
 
import javax.servlet.RequestDispatcher;
import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

@WebServlet("/list")
public class SearchUsers extends HttpServlet {
    private static final long serialVersionUID = 1L;
 
    protected void doGet(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {
        String selfEmail = request.getParameter("selfEmail");
    	UserProfileDAO dao = new UserProfileDAO();
    	Object NamLas = request.getParameter("NamLas");
    	selfEmail=selfEmail.toString();
        try {
 
            List<UserProfile> listUserProfile = dao.list(NamLas,selfEmail);
            request.setAttribute("listUserProfile", listUserProfile);
            request.setAttribute("selfEmail", selfEmail);
            RequestDispatcher dispatcher = request.getRequestDispatcher("Users.jsp");
            dispatcher.forward(request, response);
 
        } catch (SQLException e) {
            e.printStackTrace();
            throw new ServletException(e);
        }
    }
}
