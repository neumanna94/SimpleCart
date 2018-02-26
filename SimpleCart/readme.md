# _{SimpleCart}_

#### _{Will use many-many relationships to keep track of multiple users and the items they have in their respective cart. And carts that they have checked out.}, {2/26/17}_

#### By _**{Alexander Neumann, Justin Lardani, Ernest Bruno @ Epicodus}**_

## Description

_{}_

## Setup/Installation Requirements

* _Clone from Github_
* _Open phpMyAdmin, import SQL files in SimpleCart.Solution folder._
* _While in the cloned project path execute dotnet run_


## Known Bugs

_{None currently known of.}_

## Support and contact details

_{alexander.daniel.neumann@gmail.com}_

## Technologies Used

_{HTML, CSS, C#, ASP.NET MVC 1.1.3,Unit Testing, MAMP, SQL, MyPhpAdmin}_

## _{Specifications}_
## Class Name: User
_{Properties: id, name, login, password, address, email, cart}_
_{Methods: Getters, Setters, Save(), GetAll(), Find(int id), FindClients(), DeleteAll(), DeleteRow(int id), Login()}_

## Class Name: Item
_{Properties: name, description, cost, imgurl, stock}_
_{Methods: Save(), GetAll(), Find(int id), DeleteAll(), DeleteRow(int id)}_

## Class Name: Cart
_{Properties: _userId;}_
_{Methods: Save(), GetAll(), Find(int id), DeleteAll(), DeleteRow(int id) }_


### License

*{MIT}*

Copyright (c) 2018 **_{Alexander Neumann @ Epicodus}_**
