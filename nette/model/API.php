<?php

/*
 * Tým číslo 15
 * API objekt
 * Chráněno salt klíčem
 */

namespace App\Model;

use Nette,
    Nette\Http\Url;

class API {
    private $database;

    private $key;
    private $sid;
    private $tmp;
    private $vum;
    private $pocet;

    public function __construct(Nette\Database\Context $db)
    {
        $this->database = $db;
    }

    public function prirazeniParametru($key = NULL, $sid = NULL, $tmp = NULL, $vum = NULL, $pocet = NULL) {
        $this->key = $key;
        $this->sid = $sid;
        $this->tmp = $tmp;
        $this->vum = $vum;
        $this->pocet = $pocet;
    }

    public function pridatZaznam() {
        $data = array(
            "id_senzoru" => (!$this->sid) ? "Neurčeno" : $this->sid,
            "teplota" => (!$this->tmp) ? "0" : $this->tmp,
            "vlhkost" => (!$this->vum) ? "0" : $this->vum
        );

        $this->database->table("hodnoty")->insert($data);
    }

    public function vybratZaznamy() {
        header("Content-Type: application/json; charset=utf-8");
        header("Access-Control-Allow-Origin: *");
        $fpocet = ($this->pocet) ? $this->pocet : 3;
        $vratit = $this->database->table("hodnoty")->order("id DESC LIMIT ".$fpocet);

        $result = [];
        foreach ($vratit as $zaznam){
            $namereno = array("id_senzoru" => $zaznam->id_senzoru, "teplota" => $zaznam->teplota,  "vlhkost" => $zaznam->vlhkost, "datum" => $zaznam->datum);
            array_push($result, $namereno);
        }

        $jsonData = json_encode($result, JSON_PRETTY_PRINT);
        return $jsonData;
    }

    public function reset() {
        $this->database->query("TRUNCATE TABLE hodnoty");
    }

    public function overitPristup() {
        if ($this->key && $this->key == "t4m15")
            return true;
        return false;
    }
}
