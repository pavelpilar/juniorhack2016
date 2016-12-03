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
    private $l;
    private $pocet;

    private $nid;
    private $maxtmp;
    private $mintmp;
    private $maxvum;
    private $minvum;
    private $topeni;
    private $okna;

    public function __construct(Nette\Database\Context $db)
    {
        $this->database = $db;
    }

    public function prirazeniParametru($key = NULL, $sid = NULL, $tmp = NULL, $vum = NULL, $l = NULL, $pocet = NULL) {
        $this->key = $key;
        $this->sid = $sid;
        $this->tmp = $tmp;
        $this->vum = $vum;
        $this->l = $l;
        $this->pocet = $pocet;
    }

    public function prirazeniParametruNastaveni($key = NULL, $nid = NULL, $maxtmp = NULL, $mintmp = NULL, $maxvum = NULL, $minvum = NULL, $topeni = NULL, $okna = NULL) {
        $this->key = $key;
        $this->nid = $nid;
        $this->maxtmp = $maxtmp;
        $this->mintmp = $mintmp;
        $this->maxvum = $maxvum;
        $this->minvum = $minvum;
        $this->topeni = $topeni;
        $this->okna = $okna;
    }

    public function pridatZaznam() {
        $data = array(
            "id_senzoru" => (!$this->sid) ? "Neurčeno" : $this->sid,
            "teplota" => (!$this->tmp) ? "0" : $this->tmp,
            "vlhkost" => (!$this->vum) ? "0" : $this->vum,
            "svitivost" => (!$this->l) ? "0" : $this->l
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
            $namereno = array("id_senzoru" => $zaznam->id_senzoru, "teplota" => $zaznam->teplota, "vlhkost" => $zaznam->vlhkost, "datum" => $zaznam->datum);
            array_push($result, $namereno);
        }

        $jsonData = json_encode($result, JSON_PRETTY_PRINT);
        return $jsonData;
    }

    public function getOutDevice() {
        header("Content-Type: application/json; charset=utf-8");
        header("Access-Control-Allow-Origin: *");
        $vratit = $this->database->query("SELECT * FROM hodnoty WHERE id_senzoru = 'output' ORDER BY datum DESC LIMIT 10");

        $result = [];
        foreach ($vratit as $zaznam){
            $namereno = array("id_senzoru" => $zaznam->id_senzoru, "teplota" => $zaznam->teplota, "vlhkost" => $zaznam->vlhkost, "datum" => $zaznam->datum);
            array_push($result, $namereno);
        }

        $jsonData = json_encode($result, JSON_PRETTY_PRINT);
        return $jsonData;
    }

    public function reset() {
        $this->database->query("TRUNCATE TABLE hodnoty");
    }

    public function update() {
        $data = array(
            "maximalni_teplota" => (!$this->maxtmp) ? "Neurčeno" : $this->maxtmp,
            "minimalni_teplota" => (!$this->mintmp) ? "0" : $this->mintmp,
            "maximalni_vlhkost" => (!$this->maxvum) ? "0" : $this->maxvum,
            "minimalni_vlhkost" => (!$this->minvum) ? "0" : $this->minvum,
            "otevreni_oken" => (!$this->okna) ? "0" : $this->okna,
            "zapnuti_topeni" => (!$this->topeni) ? "0" : $this->topeni,
        );

        $this->database->table("nastaveni")->where(array("id" => $this->nid))->update($data);
    }

    public function overitPristup() {
        if ($this->key && $this->key == "t4m15")
            return true;
        return false;
    }
}
