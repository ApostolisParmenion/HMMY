<%@ page language="java" contentType="text/html; charset=ISO-8859-1"
    pageEncoding="ISO-8859-1" %>
<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c" %>
<!DOCTYPE html>
<html>


<head>
<meta charset="ISO-8859-1">
<title>Profile</title>
</head>
<link rel="stylesheet" href = "Style.css"/>
<body>

<div class="tableloc">
	<table>
	  <tr>
	    <th>Id</th>
	    <th>Name</th>
	    <th>Barcode</th>
	    <th>Color</th>
	    <th>Description</th>
	  </tr>
	  
	    <c:forEach items="${ProductsInfo}" var="product">
	        <tr>
	        	<th>${product.id}</th>
	    		<th>${product.name}</th>
	    		<th>${product.barcode}</th>
	    		<th>${product.color}</th>
	    		<th>${product.description}</th>
	    	</tr>
	    </c:forEach>
	</table>
</div>



<form action = "Index.jsp">
	<button class="btn" type="submit">Go to homepage!</button>
</form>
</body>
</html>