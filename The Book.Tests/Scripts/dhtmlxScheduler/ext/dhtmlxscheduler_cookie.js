/*
@license
dhtmlxScheduler.Net v.3.4.1 Professional Evaluation

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) Dinamenta, UAB.
*/
Scheduler.plugin(function(e){!function(){function t(e,t,i){var a=e+"="+i+(t?"; "+t:"");document.cookie=a}function i(e){var t=e+"=";if(document.cookie.length>0){var i=document.cookie.indexOf(t);if(-1!=i){i+=t.length;var a=document.cookie.indexOf(";",i);return-1==a&&(a=document.cookie.length),document.cookie.substring(i,a)}}return""}var a=!0;e.attachEvent("onBeforeViewChange",function(n,r,s,d){if(a&&e._get_url_nav){var o=e._get_url_nav();(o.date||o.mode||o.event)&&(a=!1)}var l=(e._obj.id||"scheduler")+"_settings";
if(a){a=!1;var h=i(l);if(h){e._min_date||(e._min_date=d),h=unescape(h).split("@"),h[0]=this.templates.xml_date(h[0]);var _=this.isViewExists(h[1])?h[1]:s,c=isNaN(+h[0])?d:h[0];return window.setTimeout(function(){e.setCurrentView(c,_)},1),!1}}var u=escape(this.templates.xml_format(d||r)+"@"+(s||n));return t(l,"expires=Sun, 31 Jan 9999 22:00:00 GMT",u),!0});var n=e._load;e._load=function(){var t=arguments;if(e._date)n.apply(this,t);else{var i=this;window.setTimeout(function(){n.apply(i,t)},1)}}}()});