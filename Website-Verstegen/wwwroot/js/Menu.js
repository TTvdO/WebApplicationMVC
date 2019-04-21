function toggleMenu() {
    var menu = String.fromCharCode(9776);
    var cross = String.fromCharCode(10006);
    if (document.getElementById("sidenavi").style.display == "") {
        if (window.innerWidth <= 767) {
            document.getElementById("sidenavi").style.display = "grid";
        } else {
            document.getElementById("sidenavi").style.display = "flex";
        }

        document.getElementById("dropdownMenuLink").style.cssText = 'background-color: white !important; color:#cd0b35;';
        document.getElementById("menubuttontoggler").style.cssText = 'background-color: white !important; color:#cd0b35 !important;';
        document.getElementById("dropdownMenuLink").innerText = cross + " Close";
        document.getElementById("menubuttontoggler").innerText = cross;
        document.getElementById("food-ind").style.cssText = 'background: rgba(0,0,0,0) !important;';
    }
    else {
        document.getElementById("dropdownMenuLink").style.cssText = 'background-color: #cd0b35 !important; color:white;';
        document.getElementById("menubuttontoggler").style.cssText = 'background-color: #cd0b35 !important; color:white;';
        document.getElementById("food-ind").style.cssText = 'background: rgba(0,0,0,0.25) !important;';
        document.getElementById("dropdownMenuLink").innerText = menu + " Menu";
        document.getElementById("menubuttontoggler").innerText = menu;
        document.getElementById("sidenavi").style.display = "";
    }
}

