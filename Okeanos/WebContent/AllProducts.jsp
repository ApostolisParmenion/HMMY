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


th, td {
  text-align: center;
  padding: 8px;
}
tr:hover {background-color:#064564;}
tr:nth-child(even) {background-color: #0a6b9a;}
</style>
<head>
<meta charset="ISO-8859-1">
<title>Profile</title>
</head>

<body>
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
<form action = "Index.jsp">
	<button type="submit">Go to homepage!</button>
</form>
</body>
</html>