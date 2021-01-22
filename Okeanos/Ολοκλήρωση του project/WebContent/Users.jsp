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

<% if(request.getParameter("selfEmail") == null){
	response.sendRedirect("Index.jsp");
	}%>
<table>
  <tr>
    <th>Name</th>
    <th>Lastname</th>
    <th>Country</th>
    <th>City</th>
    <th>Address</th>
    <th>Zipcode</th>
    <th>Phone</th>
    <th>Email</th>
  </tr>
  
    <c:forEach items="${listUserProfile}" var="profile">
        <tr>
    		<th>${profile.name}</th>
    		<th>${profile.lastname}</th>
    		<th>${profile.address}</th>
    		<th>${profile.city}</th>
    		<th>${profile.country}</th>
    		<th>${profile.zipcode}</th>
    		<th>${profile.phone}</th>
    		<th>${profile.email}</th>
    		<th><form action = "Showprofile">
	    		<input type="hidden" value="${profile.email}" name = "email">
	    		<input type = "hidden" value="${selfEmail}" name = "selfEmail"></input>
	    		<button >Show Profile</button></form>
	    	</th></tr>
    </c:forEach>
</table>
<form action = "GoHomepage">
<input type = "hidden" value="${selfEmail}" name = "selfEmail"></input>
<button type="submit">Go to homepage!</button>
</form>
</body>
</html>