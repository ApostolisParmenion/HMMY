package com.project;


public class UserProfile {
    private String name,lastname,address,city,country,zipcode,phone,email;
 
    public UserProfile(String name, String lastname,String address, String city, String country, String zipcode, String phone, String email) {
        super();
        this.name = name;
        this.lastname = lastname;
        this.address = address;
        this.city = city;
        this.country = country;
        this.zipcode = zipcode;
        this.phone = phone;
        this.email = email;
    }
    public String getName() {
        return name;
    }
 
    public void setName(String name) {
        this.name = name;
    }
   
    public String getLastname() {
        return lastname;
    }
 
    public void setLastname(String lastname) {
        this.lastname = lastname;
    }
    public String getAddress() {
        return address;
    }
 
    public void setAddress(String address) {
        this.address = address;
    }
    public String getCity() {
        return city;
    }
 
    public void setCity(String city) {
        this.city = city;
    }
    public String getCountry() {
        return country;
    }
 
    public void setCountry(String country) {
        this.country = country;
    }
    public String getZipcode() {
        return zipcode;
    }
 
    public void setZipcode(String zipcode) {
        this.zipcode = zipcode;
    }
    public String getPhone() {
        return phone;
    }
 
    public void setPhone(String phone) {
        this.phone = phone;
    }
    public String getEmail() {
        return email;
    }
 
    public void setEmail(String email) {
        this.email = email;
    }
}