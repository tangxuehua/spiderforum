﻿/* Begin Ajax */

function Ajax_GetXMLHttpRequest() {
    if (window.XMLHttpRequest) {
        return new XMLHttpRequest();
    } else {
        if (window.Ajax_XMLHttpRequestProgId) {
            return new ActiveXObject(window.Ajax_XMLHttpRequestProgId);
        } else {
            var progIds = ["Msxml2.XMLHTTP.5.0", "Msxml2.XMLHTTP.4.0", "MSXML2.XMLHTTP.3.0", "MSXML2.XMLHTTP", "Microsoft.XMLHTTP"];
            for (var i = 0; i < progIds.length; ++i) {
                var progId = progIds[i];
                try {
                    var x = new ActiveXObject(progId);
                    window.Ajax_XMLHttpRequestProgId = progId;
                    return x;
                } catch (e) {
                }
            }
        }
    }
    return null;
}
function Ajax_CallBack(type, id, method, args, clientCallBack, debugRequestText, debugResponseText, debugErrors, includeControlValuesWithCallBack, url) {
    if (!url) {
        url = window.location.href;
        url = url.replace(/\#.*$/, '');
        if (url.indexOf('?') > -1)
            url += "&Ajax_CallBack=true";
        else {
            if (url.substr(url.length - 1, 1) == "/")
                url += "default.aspx";

            url += "?Ajax_CallBack=true";
        }
    }
    var x = Ajax_GetXMLHttpRequest();

    var result = null;
    if (!x) {
        result = { "value": null, "error": "NOXMLHTTP" };
        if (debugErrors) {
            alert("error: " + result.error);
        }
        if (clientCallBack) {
            clientCallBack(result);
        }
        return result;
    }

    x.open("POST", url, clientCallBack ? true : false);
    x.setRequestHeader("Content-Type", "application/x-www-form-urlencoded; charset=utf-8");
    if (clientCallBack) {
        x.onreadystatechange = function() {
            var result = null;

            if (x.readyState != 4) {
                return;
            }

            if (debugResponseText) {
                alert(x.responseText);
            }

            try {
                var result = eval("(" + x.responseText + ")");
                if (debugErrors && result.error) {
                    alert("error: " + result.error);
                }
            }
            catch (err) {
                if (window.confirm('The following error occured while processing an AJAX request: ' + err.message + '\n\nWould you like to see the response?')) {
                    var w = window.open();
                    w.document.open('text/plain');
                    w.document.write(x.responseText);
                    w.document.close();
                }

                result = new Object();
                result.error = 'An AJAX error occured.  The response is invalid.';
            }

            clientCallBack(result);
        };
    }
    var encodedData = "Ajax_CallBackType=" + type;
    if (id) {
        encodedData += "&Ajax_CallBackId=" + id.split("$").join(":");
    }
    encodedData += "&Ajax_CallBackMethod=" + method;
    if (args) {
        for (var i in args) {
            encodedData += "&Ajax_CallBackArgument" + i + "=" + encodeURIComponent(args[i]);
        }
    }
    if (includeControlValuesWithCallBack && document.forms.length > 0) {
        var form = document.forms[0];
        for (var i = 0; i < form.length; ++i) {
            var element = form.elements[i];
            if (element.name) {
                var elementValue = null;
                if (element.nodeName == "INPUT") {
                    var inputType = element.getAttribute("TYPE").toUpperCase();
                    if (inputType == "TEXT" || inputType == "PASSWORD" || inputType == "HIDDEN" || inputType == "FILE") {
                        elementValue = element.value;
                    } else if (inputType == "CHECKBOX" || inputType == "RADIO") {
                        if (element.checked) {
                            elementValue = element.value;
                        }
                    }
                } else if (element.nodeName == "SELECT") {
                    elementValue = element.value;
                } else if (element.nodeName == "TEXTAREA") {
                    elementValue = element.value;
                }
                if (elementValue) {
                    encodedData += "&" + element.name + "=" + encodeURIComponent(elementValue);
                }
            }
        }
    }
    if (debugRequestText) {
        alert(encodedData);
    }
    x.send(encodedData);
    if (!clientCallBack) {
        if (debugResponseText) {
            alert(x.responseText);
        }
        result = eval("(" + x.responseText + ")");
        if (debugErrors && result.error) {
            alert("error: " + result.error);
        }
    }
    delete x;
    return result;
}

/* End Ajax */

function checkPositiveNumber(value)
{  
    var re = /^[1-9]+[0-9]*]*$/;  
    return re.test(value);
} 

//Convert HTML text to none HTML text.
String.prototype.toText = function() {
    var tmpDiv = document.createElement("div");
    tmpDiv.innerHTML = this;
    var tmpTxt = "";
    if (document.all) {
        tmpTxt = tmpDiv.innerText;
    }
    else {
        tmpTxt = tmpDiv.textContent;
    }
    tmpDiv = null;
    return tmpTxt;
};
// Trim() , Ltrim() , RTrim()
String.prototype.Trim = function() {
    return this.replace(/(^\s*)|(\s*$)/g, "");
};
String.prototype.LTrim = function() {
    return this.replace(/(^\s*)/g, "");
};
String.prototype.RTrim = function() {
    return this.replace(/(\s*$)/g, "");
};
function GetCommonText(obj) {
    if(obj != undefined){
        if (obj.innerText) { return obj.innerText; }
        else { return obj.textContent; }
    }
}
// addEvent
function addEvent(obj, type, fn) {
    if (obj.addEventListener) {
        obj.addEventListener(type, fn, false);
    }
    else if (obj.attachEvent) {
        obj.attachEvent("on" + type, fn);
    }
    else {
        obj["on" + type] = fn;
    }
}
// removeEvent
function removeEvent(obj, type, fn) {
    if (obj.removeEventListener) obj.removeEventListener(type, fn, false);
    else if (obj.detachEvent) {
        obj.detachEvent("on" + type, fn);
    }
    else {
        obj["on" + type] = null;
    }
}
function encodeParse(obj) {
    if ((obj == null) || (obj.value == "")) {
        return false;
    }
    var str = obj.value;
    var result = "";
    for (var i = 0; i < str.length; i++) {
        var c = str.charCodeAt(i);
        if (c == 12288) {
            result += String.fromCharCode(32);
            continue;
        }
        if (c > 65280 && c < 65375) {
            result += String.fromCharCode(c - 65248);
            continue;
        }
        result += String.fromCharCode(c);
    }
    obj.value = result;

    return true;
}
function getObject(id) {
    if (document.getElementById(id)) {
        return document.getElementById(id);
    } else if (document.all) {
        return document.all[id];
    } else if (document.layers) {
        return document.layers[id];
    }
}

//Auto set the height of iframe.
function setIFrameHeight(iframeObj) {
    var newHeight = null;
    var minHeight = 800;
    var extraHeight = 50;

    if (iframeObj) {
        if (iframeObj.contentDocument && iframeObj.contentDocument.body.clientHeight) {
            newHeight = iframeObj.contentDocument.body.clientHeight;
            iframeObj.height = newHeight < minHeight ? minHeight + extraHeight : newHeight + extraHeight;
        } else if (parent.document.frames[iframeObj.name].document && parent.document.frames[iframeObj.name].document.body.clientHeight) {
            newHeight = parent.document.frames[iframeObj.name].document.body.clientHeight;
            iframeObj.height = newHeight < minHeight ? minHeight + extraHeight : newHeight + extraHeight;
        }
    }
}

function getSubString(totalString, startSubString, endCharactor)
{
    var startIndex = totalString.indexOf(startSubString);
    var valueStartIndex = null;
    var currentIndex = null;
    var currentChar = null;
    
    if (startIndex > 0)
    {
        valueStartIndex = startIndex + startSubString.length;
        if (endCharactor == null || endCharactor == "")
        {
            return totalString.substring(valueStartIndex);
        }
        currentIndex = valueStartIndex;
        currentChar = totalString.substring(currentIndex, currentIndex + 1);
        while (currentChar != endCharactor && currentChar != "")
        {
            currentIndex = currentIndex + 1;
            currentChar = totalString.substring(currentIndex, currentIndex + 1);
        }
        
        return totalString.substring(valueStartIndex, currentIndex);
    }
    
    return null;
}

function redirectToFirstPage()
{
    var href = window.location.href;  
    var availableChars = "0123456789&";
    var pageIndexParamStartIndex = href.indexOf("PageIndex=");
    var currentChar = null;
    var pageIndexParamValueStartIndex = null;
    
    if (pageIndexParamStartIndex > 0)
    {
        pageIndexParamValueStartIndex = pageIndexParamStartIndex + 10;
        currentChar = href.substring(pageIndexParamValueStartIndex, pageIndexParamValueStartIndex + 1);
        while (availableChars.indexOf(currentChar) > 0)
        {
            pageIndexParamValueStartIndex = pageIndexParamValueStartIndex + 1;
            currentChar = href.substring(pageIndexParamValueStartIndex, pageIndexParamValueStartIndex + 1);
            if (currentChar == "&" || currentChar == "")
            {
                break;
            }
        }
        var pageIndexParamAndValue = href.substring(pageIndexParamStartIndex, pageIndexParamValueStartIndex);
        var newHref = href.replace(pageIndexParamAndValue, "PageIndex=1");
        if (newHref != href)
        {
            window.location = newHref;
        }
        else
        {
            if (href.indexOf("?PageIndex=1&") > 0)
            {
                window.location = href.replace("?PageIndex=1&", "?");
            }
            else if (href.indexOf("?PageIndex=1") > 0)
            {
                window.location = href.replace("?PageIndex=1", "");
            }
            else if (href.indexOf("&PageIndex=1") > 0)
            {
                window.location = href.replace("&PageIndex=1", "");
            }
            else
            {
                window.location.reload();
            }
        }
    }
    else
    {
        if (href.indexOf("?") > 0)
        {
            window.location = href + "&PageIndex=1";
        }
        else
        {
            window.location = href + "?PageIndex=1";
        }
    }
}

var BbsList={
    init:function(){
        Userinfo.loadUserinfo()
    },
    toggle:function(A){
        obj=document.getElementById(A);
        obj.style.display=(obj.style.display=="")?"none":"";
    },
    changeIframe:function(){
        BbsList.toggle("leftBar");
        var B=document.getElementById("changeImg");
        var A=document.getElementById("mainArea");
        B.src=(B.src.indexOf("left")!=-1)?B.src.replace("left","right"):B.src.replace("right","left");
        A.style.marginLeft=(A.style.marginLeft=="0px")?"160px":"0px";
    },
    onTabChange:function(D,E,A,C){
        var B=1;while(B<=C){
            if(B!=A){
                href_obj=document.getElementById(D+B);
                if(href_obj!=null){
                    href_obj.className="";
                }
                div_obj=document.getElementById(E+B);
                if(div_obj!=null){
                    div_obj.style.display="none";
                }
            }
            B=B+1;
        }
        href_obj=document.getElementById(D+A);
        if(href_obj!=null){
            href_obj.className="active";
        }
        div_obj=document.getElementById(E+A);
        if(div_obj!=null){
            div_obj.style.display="block";
        }
    }
};