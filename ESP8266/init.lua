print(wifi.sta.getip())
--nil
wifi.setmode(wifi.STATION)
wifi.sta.config("cichnovabrno.public","cichnovabrno")
print(wifi.sta.getip())
--192.168.18.110