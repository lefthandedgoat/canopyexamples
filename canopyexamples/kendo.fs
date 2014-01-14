module kendo
open canopy
open runner
open OpenQA.Selenium

let address = "http://demos.kendoui.com/web/grid/index.html"

//selectors
let currentPage = "#grid .k-state-selected"
let next = "#grid .k-i-arrow-e"
let last = "#grid .k-i-seek-e"
let previous = "#grid .k-i-arrow-w"
let first = "#grid .k-pager-first"
let pageArrow = "#grid .k-select"
let customers = "#grid tr[role='row']"
let productName = "input[name='ProductName']"
let discontinued = "input[name='Discontinued']"
let filterFreight = "th[data-field='Freight'] .k-filter"
let filterValue = "(//span[@class='k-widget k-numerictextbox']//input)[1]"
let filterRealValue = "(//span[@class='k-widget k-numerictextbox']//input)[2]"

// helpers
let contact ofRow = sprintf "#grid tr[role='row']:nth-of-type(%i) td:nth-of-type(1)" ofRow
let option ofValue = sprintf "//div[@class='k-animation-container km-popup']//li[text()='%i']" ofValue
let edit ofRow = sprintf "#grid tr:nth-of-type(%i) a:nth-of-type(1)" ofRow
let up (for' : string) = sprintf "div[data-container-for = '%s'] .k-i-arrow-n" (for'.Replace(" ", ""))
let down (for' : string) = sprintf "div[data-container-for = '%s'] .k-i-arrow-s" (for'.Replace(" ", ""))

//optional for use with f# interactive
//#r @"C:\projects\canopyexamples\canopyexamples\packages\canopy.0.9.1\lib\canopy.dll"
//#r @"C:\projects\canopyexamples\canopyexamples\packages\Newtonsoft.Json.5.0.8\lib\net45\Newtonsoft.Json.dll"
//#r @"C:\projects\canopyexamples\canopyexamples\packages\Selenium.WebDriver.2.39.0\lib\net40\WebDriver.dll"
//#r @"C:\projects\canopyexamples\canopyexamples\packages\Selenium.Support.2.39.0\lib\net40\WebDriver.Support.dll"
//#r @"C:\projects\canopyexamples\canopyexamples\packages\SizSelCsZzz.0.3.35.0\lib\SizSelCsZzz.dll"

//test which you can move to another file if you want to
let all _ =    
    once (fun _ -> url address)

    "sorting" &&& fun _ ->
        url address

        //you can test this many ways.  here we know we have a static set of test data, so we can check specific values
        //if you wanted you could pull the values out and sort them with code to ensure that grid sorting is working on a dynamic set of data
        
        //asc
        click "Contact Name"
        contact 1 == "Alejandra Camino"
        contact 2 == "Alexander Feuer"
        contact 10 == "Art Braunschweiger"

        //desc
        click "Contact Name"
        contact 1 == "Zbyszek Piestrzeniewicz"
        contact 2 == "Yvonne Moncada"
        contact 10 == "Sergio Gutiérrez"

        //default
        click "Contact Name"
        contact 1 == "Maria Anders"
        contact 2 == "Ana Trujillo"
        contact 10 == "Elizabeth Lincoln"

    "paging" &&& fun _ ->
        currentPage == "1"
        click next
        currentPage == "2"
        click last
        currentPage == "10"
        click previous
        currentPage == "9"
        click first
        currentPage == "1"

    "items per page" &&& fun _ ->
        click pageArrow
        click (option 5)
        count customers 5
        
        click "Contact Name"//work around for it not allowing you to click it immediately after, til I figure something else out better
        click pageArrow
        click (option 10)
        count customers 10

        click "Contact Name"
        click pageArrow
        click (option 20)
        count customers 20
    
    "edit a product" &&& fun _ ->
        click "Popup editing"
        
        sleep 1 //work around again
        click (edit 1)
        productName << "Chai Tea"
        click (up "Unit Price")
        click (down "Units In Stock")
        check discontinued
        click "Update"
        "td" *= "Chai Tea"

    "filtering" &&& fun _ ->
        click "Binding to remote data"
        click filterFreight
        sleep 1 //and again
        click filterValue
        filterRealValue << "51.3"
        click "Filter"