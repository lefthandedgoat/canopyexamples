module telerikWebforms

open canopy
open runner

let address = "http://demos.telerik.com/aspnet-ajax/grid/examples/overview/defaultcs.aspx"

//selectors
let demoName = "span[id $= '_DemoName']"
let currentPage = ".rgCurrentPage"
let next = ".rgPageNext"
let last = ".rgPageLast"
let previous = ".rgPagePrev"
let first = ".rgPageFirst"
let pageArrow = "a[id $= '_PageSizeComboBox_Arrow']"
let cars = ".rgRow,.rgAltRow"
let dateOfRent = "a[id $= '_DateOfRentPicker_popupButton']"
let returnDate = "a[id $= '_ReturnDatePicker_popupButton']"
let firstName = "input[id $= '_FirstNameTextBox']"
let lastName = "input[id $= '_LastNameTextBox']"
let email = "input[id $= '_EmailTextBox']"
let popup = "div[id $= '_RadWindow1']"
let filterModelText = "input[id $= '_FilterTextBox_Model']"
let filterModel = "input[id $= '_Filter_Model']"

// helpers
let brand ofRow = sprintf "tr[id $= '_RadGrid1_ctl00__%i'] td:nth-of-type(2)" (ofRow - 1)
let bookCar ofRow = sprintf "tr[id $= '_RadGrid1_ctl00__%i'] td:nth-of-type(9)" (ofRow - 1) |> click

//test which you can move to another file if you want to
let all _ =    
    once (fun _ -> url address)
    
    "click load gives a null ref yellow screen of death" &&& fun _ ->
        //sorry thought it was funny
        click "Load"
        "i" == "Object reference not set to an instance of an object."
        url address

    "navigation" &&& fun _ ->
        click "Data Binding"
        click "Client-side binding"
        
        click "Declarative Binding"
        demoName == "Grid - Declarative Binding"

        click "Programmatic Binding"
        demoName == "Grid - Programmatic Binding"

        click "ClientItemTemplate"
        demoName == "Grid - ClientItemTemplate"

    "sorting" &&& fun _ ->
        url address

        //you can test this many ways.  here we know we have a static set of test data, so we can check specific values
        //if you wanted you could pull the values out and sort them with code to ensure that grid sorting is working on a dynamic set of data
        
        //asc
        click "Brand Name"
        brand 1 == "Alfa Romeo"
        brand 2 == "Alfa Romeo"
        brand 10 == "BMW"

        //desc
        click "Brand Name"
        brand 1 == "VW"
        brand 2 == "VW"
        brand 10 == "Volvo"

        //default
        click "Brand Name"
        brand 1 == "Opel"
        brand 2 == "Honda"
        brand 10 == "Honda"

    "paging" &&& fun _ ->
        currentPage == "1"
        click next
        currentPage == "2"
        click last
        currentPage == "6"
        click previous
        currentPage == "5"
        click first
        currentPage == "1"

    "cars per page" &&& fun _ ->
        click pageArrow
        click "5"
        count cars 5
        
        click pageArrow
        click "10"
        count cars 10
    
    "book a car" &&& fun _ ->
        bookCar 1
        
        click dateOfRent
        click "20"
        click returnDate
        click "22"
        firstName << "Bob"
        lastName << "Barker"
        email << "bobbarker@thepriceisright.com"

        click "Book Now"
        click "Close"
        notDisplayed popup

    "filtering" &&& fun _ ->
        filterModelText << "Civ"
        click filterModel
        click "Contains"        
        click filterModel

        count cars 3