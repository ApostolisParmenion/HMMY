package com.project;

import java.io.IOException;

import javax.servlet.RequestDispatcher;
import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

@WebServlet("/GoHomepage")
public class GoHomepage extends HttpServlet {
	private static final long serialVersionUID = 1L;

    public GoHomepage() {
        super();

    }

	protected void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		String selfEmail = request.getParameter("selfEmail");
		if (selfEmail.contentEquals("admin1234")) {
			request.setAttribute("selfEmail", selfEmail);
			RequestDispatcher rs = request.getRequestDispatcher("HomepageAdmin.jsp");
	        rs.include(request, response);
		}
		else {
		request.setAttribute("selfEmail", selfEmail);
		RequestDispatcher rs = request.getRequestDispatcher("Homepage.jsp");
        rs.include(request, response);
		}
	}
}
