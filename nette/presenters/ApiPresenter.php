<?php

/*
 * Tým číslo 15
 * API pro přidávání do databáze
 * Chráněno salt klíčem
 */

namespace App\Presenters;

use Nette,
    Nette\Http\Url,
    Nette\Database\Context;

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

    public function renderDefault($key = NULL, $sid = NULL, $tmp = NULL, $vum = NULL, $l = NULL, $ziskat)
    {
        // Přiřazení proměnných
        $this->template->key = $key;
        $this->template->sid = $sid;
        $this->template->tmp = $tmp;
        $this->template->vum = $vum;
        $this->template->l = $l;
        $this->template->ziskat = $ziskat;

        $this->pridatZaznam();

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
        $this->db->query("SELECT * FROM hodnoty")->dump();
    }

    private function ziskatZaznamy($pocet) {
        if (is_numeric($pocet))
            return $pocet;
        return "Hodnota parametru není číselná!";
    }
}

