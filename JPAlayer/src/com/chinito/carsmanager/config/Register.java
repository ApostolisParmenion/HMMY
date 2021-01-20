package com.chinito.carsmanager.config;


import java.io.*;
import java.sql.*;
import java.util.ArrayList;
import java.util.List;


import java.io.IOException;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
 
import javax.servlet.RequestDispatcher;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import javax.persistence.EntityManagerFactory;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.data.jpa.repository.config.EnableJpaRepositories;
import org.springframework.orm.jpa.JpaTransactionManager;
import org.springframework.orm.jpa.LocalEntityManagerFactoryBean;

import com.chinito.catsmanager.model.ProductsInfo;

import java.sql.Connection;

@Configuration
@EnableJpaRepositories (basePackages = {"com.chinito.carsmanager"})

public class Register extends HttpServlet {
	private static final long serialVersionUID = 1L;

    public Register() {
        super();

    }
    
    public LocalEntityManagerFactoryBean entityManagerFactory() {
	    LocalEntityManagerFactoryBean factoryBean = new LocalEntityManagerFactoryBean();
	    factoryBean.setPersistenceUnitName("CarsDB");

    return factoryBean;
    }

    public JpaTransactionManager transactionManager (EntityManagerFactory entityManagerFactory) {
	    JpaTransactionManager transactionManager = new JpaTransactionManager();
	    transactionManager.setEntityManagerFactory(entityManagerFactory);

    return transactionManager;

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
        	List<ProductsInfo> Products= new ArrayList<>();
        	ProductsInfo temp;
        	c = DriverManager.getConnection("jdbc:postgresql://localhost:5432/postgres","postgres", "theologis123");
	        
	        stmt = c.createStatement();
	        
	        stmt = c.createStatement();
	        String sql = "select count(*)" + 
	        		"from public.Products U " + 
	        		"where '"+barcode+"' in (select barcode from public.Products US where U.barcode=US.barcode)";
	        ResultSet rs = stmt.executeQuery(sql);
	        rs.next();
	        Integer validnum = rs.getInt("count");
	        if (validnum==0){
	            PreparedStatement ps = c.prepareStatement
	                        ("INSERT INTO public.Products(name, barcode, color,description) values(?,?,?,?)");
	            ps.setString(1, name);
	            ps.setInt(2, barcode);
	            ps.setString(3,color);
	            ps.setString(4, description);
	            

	  
	            int i = ps.executeUpdate();
	            
	            if(i > 0 ) {
	            	String entoli = "SELECT * FROM Products";
	                Statement dilwsh = c.createStatement();
	                ResultSet data = dilwsh.executeQuery(entoli);
	                while (data.next()) {
	     			    String Name = data.getString("name");
	     			    Integer Barcode = data.getInt("barcode");
	     			    String Color = data.getString("color");
	     			    temp= new ProductsInfo(Name,Barcode,Color,description);
	     			    Products.add(temp);
	                }
	     			    
	     			    
	     			    
	                request.setAttribute("ProductsInfo", Products);
	                RequestDispatcher dispatcher = request.getRequestDispatcher("AllProducts.jsp");
	                dispatcher.forward(request, response);
	            }
	        }
	        else {
	        	out.println("Please give another barcode");
	        	RequestDispatcher ns = request.getRequestDispatcher("Index.jsp");
	            ns.include(request, response);
	        }
	        stmt.close();
			c.close();
        
        } catch (SQLException e) {
            e.printStackTrace();
            throw new ServletException(e);
        } catch (ClassNotFoundException e) {
			e.printStackTrace();
        }
	
    }
}