var page = require('webpage').create();
var system = require('system');
var fs = require('fs');

var address = null;
var output = null;
var idToSave = null;

if (system.args.length >= 3){
    address = system.args[1];
    output = system.args[2];
}else {
    console.log('At least source and target address should be given.');
    phantom.exit(1);
}
    
if(system.args.length >= 4)
    idToSave = system.args[3];

console.log(address);
console.log(output);
console.log(idToSave);

page.open(address, function (status) {
    var id = idToSave;

    if (status !== 'success') {
        console.log('Unable to load the address!');
        phantom.exit(1);
    } else {
        var table_html = page.evaluate(function (id, page) {
            if (!id || id=='null' || id.length <= 0) //an element id is specified
            {
                return page.content;
            }

            console.log('id:' + id);

            var el = document.getElementById(id);
            if (!el) {
                console.log('Unable to find the element with ID ' + id);
                phantom.exit(1);
            } else {
                return el.outerHTML;
            }
        }, idToSave, page);

        fs.write(output, table_html, 'w');
        phantom.exit();
    }
});

