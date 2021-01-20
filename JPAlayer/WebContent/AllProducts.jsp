<%@ page language="java" contentType="text/html; charset=ISO-8859-1"
    pageEncoding="ISO-8859-1" %>
<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c" %>
<!DOCTYPE html>
<html>
<style>
body {
  background-image: url('photos/wel.jpg');
  background-repeat: no-repeat;
  background-attachment: fixed;
  background-size: cover;
}


#customers tr:nth-child(even){background-color: #f2f2f2;}

#customers tr:hover {background-color: #ddd;}

#customers th {
  padding-top: 12px;
  padding-bottom: 12px;
  text-align: left;
  background-color: #4CAF50;
  color: white;
}
</style>
<head>
<meta charset="ISO-8859-1">
<title>Profile</title>
</head>

<body>
<table>
  <tr>
    <th>Name</th>
    <th>Barcode</th>
    <th>Color</th>
    <th>Description</th>
  </tr>
  
    <c:forEach items="${ProductsInfo}" var="product">
        <tr>
    		<th>${product.name}</th>
    		<th>${product.barcode}</th>
    		<th>${product.color}</th>
    		<th>${product.description}</th>
    	</tr>
    </c:forEach>
</table>
<form action = "Index.jsp">
	<button type="submit">Go Back!</button>
</form>
</body>
</html>