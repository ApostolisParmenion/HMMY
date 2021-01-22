package com;


import java.io.IOException;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.List;
 
import javax.servlet.RequestDispatcher;
import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.sql.Connection;

@WebServlet("/list")


public class ShowProducts extends HttpServlet {
    private static final long serialVersionUID= 1L;
 
    protected void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
        try {
        	Class.forName("org.postgresql.Driver");
        	List<ProductsInfo> Products= new ArrayList<>();
        	ProductsInfo temp;
        	Connection c = DriverManager.getConnection("jdbc:postgresql://localhost:5432/postgres","postgres", "parmen1234");
            
	        String sql = "SELECT * FROM Products ORDER BY id";
            Statement statement = c.createStatement();
            ResultSet ps = statement.executeQuery(sql);
            while (ps.next()) {
            	Integer id= ps.getInt("id");
 			    String name = ps.getString("name");
 			    Integer barcode = ps.getInt("barcode");
 			    String color = ps.getString("color");
 			    String description = ps.getString("description");
 			    temp= new ProductsInfo(id,name,barcode,color,description);
 			    Products.add(temp);
            }
 			    
 			    
 			    
            request.setAttribute("ProductsInfo", Products);
            RequestDispatcher dispatcher = request.getRequestDispatcher("AllProducts.jsp");
            dispatcher.forward(request, response);
 
        } catch (SQLException e) {
            e.printStackTrace();
            throw new ServletException(e);
        } catch (ClassNotFoundException e) {
			e.printStackTrace();
		}
    }
}
