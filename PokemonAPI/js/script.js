
let favorites = [];
const favKey = "nhk3466_favorites";
const searchKey = "nhk3466_search";

window.addEventListener("load", displaySetUp);
window.addEventListener("load", function(){
    let button = document.querySelector("#searchbutton");
    button.onclick = searchButtonClicked;

    let star = document.querySelector("#favstar");
    star.onclick = addFavorite;

    let searchField = document.querySelector("#searchterm");
    searchField.onchange = e=>{ localStorage.setItem(searchKey, e.target.value); };
})

//load favs
window.addEventListener("load", function(){
    let favString = localStorage.getItem(favKey);
    if (favString != null)
    {
        favorites = JSON.parse(favString);
    }

    let searchField = document.querySelector("#searchterm");
    let searchString = this.localStorage.getItem(searchKey);
    if (searchString != null)
    {
        searchField.value = searchString;
    }
    
    
});

window.addEventListener("load", searchButtonClicked);


function displaySetUp()
{

    let container = document.querySelector("#pokecontainer");

    const url = "https://pokeapi.co/api/v2/pokemon?limit=1118&offset=0";

    let xhr = new XMLHttpRequest();

    // function on load
    xhr.onload = function(e){
        let obj = JSON.parse(e.target.responseText);

        // build favs first
        for (let i = 0; i < favorites.length; i++) {
            let newxhr = new XMLHttpRequest();

            newxhr.onload = function(e){
                let poke = JSON.parse(e.target.responseText);

                let d = document.createElement("div");
                d.classList.add("poke");

                //adds star when div is created
                d.innerHTML = `<img class="pokestar" src="images/star.png" alt="star"><img src="${poke.sprites.front_default}" alt="Pokemon #${poke.id}"><p>#${poke.id}</p>`

                d.addEventListener("click", function(){pokeButtonClicked(d)});

                container.appendChild(d);
                
            }

            newxhr.open("GET",obj.results[favorites[i] - 1].url);
            newxhr.send();

        }

        //build rest
        for (let i = 0; i < 1017; i++) {

            if (!favorites.includes(i+1))
            {
                let newxhr = new XMLHttpRequest();

                newxhr.onload = function(e){
                    let poke = JSON.parse(e.target.responseText);
    
                    let d = document.createElement("div");
                    d.classList.add("poke");
                    d.innerHTML = `<img src="${poke.sprites.front_default}" alt="Pokemon #${poke.id}"><p>#${poke.id}</p>`
    
                    d.addEventListener("click", function(){pokeButtonClicked(d)});
    
                    container.appendChild(d);
                    
                }
    
                newxhr.open("GET",obj.results[i].url);
                newxhr.send();
            }
            

        }

    };

    // open connection and send request
    xhr.open("GET",url);
    xhr.send();
}


// 3
function searchButtonClicked(){

    const POKE_URL = "https://pokeapi.co/api/v2/pokemon/";


    //3 build url string
    let url = POKE_URL;

    //4 parse user entered term to search
    let term = document.querySelector("#searchterm").value;
    displayTerm = term;

    //5 gets rid of any potential spaces at the front or end
    term = term.trim();

    //6 encodes spaces/special chars
    term = encodeURIComponent(term);

    //7 exits function if there is no term
    if (term.length < 1) return;

    //8 add term to url
    url += term.toLowerCase();

    // 12 Request data!
    getData(url);
    
}

function pokeButtonClicked(div){
    const POKE_URL = "https://pokeapi.co/api/v2/pokemon/";

    let url = POKE_URL;

    let id = div.querySelector("p").innerHTML.slice(1);
  
    url += id;


    getData(url);
}


function getData(url) {
    let xhr = new XMLHttpRequest();

    // function on load
    xhr.onload = dataLoaded;

    // function if error
    xhr.onerror = dataLoaded;
    

    // open connection and send request
    xhr.open("GET",url);
    xhr.send();
  
}

function dataLoaded(e){
    let xhr = e.target; 
   
    if (xhr.status == 404)
    {
        // change visual here
        //console.log("literally no way to handle this error that i could find");

        let head = document.querySelector("#head");
        head.innerHTML = "INVALID POKEMON, PLEASE RE-ENTER REQUEST";
        head.style.color="red";
        return;
    }

    let obj = JSON.parse(xhr.responseText);

    // create types
    setUpTypes(obj.types);


    createStats(obj.stats);

    // create moves
    setUpMoves(obj.moves);

    // change pokemon description
    updateDescription(obj.species.url);

    // change sprite image
    let img = document.querySelector("#spriteimg");
    if (document.querySelector("#shiny").checked)
    {
        img.src = obj.sprites.other["official-artwork"].front_shiny;
    }
    else
    {
        img.src = obj.sprites.other["official-artwork"].front_default;
    }

    //update star
    let star = document.querySelector("#favstar");
    if (favorites.includes(obj.id))
    {
        star.style.filter = "sepia(1) saturate(5)";
        
    }
    else
    {
        star.style.filter = "brightness(0.4)";
    }

   
    let head = document.querySelector("#head");

    head.style.color="white";
    head.innerHTML = obj.name.toUpperCase() + " | #" + obj.id;
    head.value = obj.id;


}

function updateDescription(url)
{

    let xhr = new XMLHttpRequest();
    
    xhr.onload = function(e) {
        let obj = JSON.parse(e.target.responseText);

        let flavortext = document.querySelector("#flavortext");

        for(let i = Object.keys(obj).length - 1; i >= 0; i--)
        {
            if (obj.flavor_text_entries[i].language.name == "en")
            {
                flavortext.innerHTML = obj.flavor_text_entries[i].flavor_text;
                return;
            }
        }

        
    }

    xhr.open("GET", url);
    xhr.send();
}

function setUpMoves(moves){

    let line = "";

    let selectedAbilities = document.querySelector("#abilities").value;
    if (selectedAbilities > moves.length)
    {
        selectedAbilities = moves.length;
    }

    document.querySelector("#moveContainer").innerHTML = "";

    for (let i = 0; i < selectedAbilities; i++)
    {
        getMoveLine(moves[i].move.url, function(linePart) {
            document.querySelector("#moveContainer").innerHTML += linePart;

            line += linePart;
        });
    }   
    
}

function getMoveLine(url, callback)
{
    

    let line = ""
    let xhr = new XMLHttpRequest();
    xhr.onload = function(e) {
        let obj = JSON.parse(e.target.responseText);

        line += '<div class="move"><p>' + obj.name.toUpperCase() + '</p><span><div>PP: ' + obj.pp + '</div><div>POWER: ' + obj.power + '</div></span></div>'

        callback(line);


    }

    xhr.open("GET", url);
    xhr.send();

}


function setUpTypes(types)
{
    let line = "";

    if (types.length > 0)
    {
        for (let i = 0; i < types.length; i++)
        {
            let name = types[i].type.name
            line += '<div class="type" style="background-color:#' + getTypeColor(name) + '">'
            line += name.toUpperCase() + '</div>'
        }


        document.querySelector("#typeHolder").innerHTML = line
        document.querySelector("#sprite").style.background = `linear-gradient(rgb(255, 255, 255), #${getTypeColor(types[0].type.name)})`;
    }
}

function createStats(stats){
    let container = document.querySelector("#statContainer");

    let line = ""

    for (let i=0; i < stats.length; i++)
    {
        line += '<div class="stat">' + stats[i].stat.name.toUpperCase() + ": " + stats[i].base_stat + '</div>';
    }

    container.innerHTML = line;
}

function addFavorite()
{
    let id = document.querySelector("#head").value;
    let star = document.querySelector("#favstar");

    if (favorites.includes(id))
    {
        favorites.splice(favorites.indexOf(id), 1);
        star.style.filter = "brightness(0.4)";
    }
    else
    {
        favorites.push(id);
        star.style.filter = "sepia(1) saturate(5)";

    }

    let container = document.querySelector("#pokecontainer");
    container.innerHTML = "";
    displaySetUp();

    // store fav data
    let stringArray = JSON.stringify(favorites)
    localStorage.setItem("nhk3466_favorites", stringArray);

 
}

function getTypeColor(type)
{
    switch (type){
        case "normal":
            return "A8A77A";
        case "fire":
            return "EE8130";
        case "water":
            return "6390F0";
        case "electric":
            return "F7D02C";
        case "grass":
            return "7AC74C";
        case "ice":
            return "96D9D6";
        case "fighting":
            return "C22E28";
        case "poison":
            return "A33EA1";
        case "ground":
            return "E2BF65";
        case "flying":
            return "A98FF3";
        case "psychic":
            return "F95587";
        case "bug":
            return "A6B91A";
        case "rock":
            return "B6A136";
        case "ghost":
            return "735797";
        case "dragon":
            return "6F35FC";
        case "dark":
            return "705746";
        case "steel":
            return "B7B7CE";
        case "fairy":
            return "D685AD";
        
    }
}



