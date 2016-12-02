<?php

/*
 * Tým číslo 15
 * API pro přidávání do databáze
 * Chráněno salt klíčem
 */

namespace App\Presenters;

use Nette,
    Nette\Http\Url;

class ApiPresenter extends Nette\Application\UI\Presenter
{
    // Parametry v URL
    /** @persistent */
    public $key;
    /** @persistent */
    public $sid;
    /** @persistent */
    public $tmp;
    /** @persistent */
    public $vum;
    /** @persistent */
    public $l;
    /** @persistent */
    public $ziskat;

    private $database;

    public function __construct(Nette\Database\Context $db)
    {
        $this->database = $db;
    }

    public function renderDefault($key = NULL, $sid = NULL, $tmp = NULL, $vum = NULL, $l = NULL, $ziskat)
    {
        // Přiřazení proměnných
        $this->template->key = $key;
        $this->template->sid = $sid;
        $this->template->tmp = $tmp;
        $this->template->vum = $vum;
        $this->template->l = $l;
        $this->template->ziskat = $ziskat;

        // Ziskani dat
        if ($this->ziskat) {
            echo $this->ziskatZaznamy($this->ziskat);
            return;
        }

        // Přístup povolen, skey zadán a zadán správně
        if ($this->overitPristup()) {
            $this->pridatZaznam();
        }
        // Nezadán security key, nepovolený přístup
        else {
            echo "Přístup odepřen!";
        }
    }

    private function overitPristup() {
        if ($this->key && $this->key == "t4m15")
            return true;
        return false;
    }

    private function pridatZaznam() {
        $data = array(
            "id_senzoru" => $this->sid,
            "teplota" => $this->tmp,
            "vlhkost" => $this->vum,
            "svitivost" => $this->l
        );

        $this->database->table("hodnoty")->insert($data);
    }

    private function ziskatZaznamy($pocet) {
        if (is_numeric($pocet))
            return $pocet;
        return "Hodnota parametru není číselná!";
    }
}

