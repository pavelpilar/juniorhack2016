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
    public $skey;
    /** @persistent */
    public $id_senzoru;
    /** @persistent */
    public $teplota;
    /** @persistent */
    public $vlhkost;

    public function renderDefault($skey = NULL, $id_senzoru = NULL, $teplota = NULL, $vlhkost = NULL)
    {
        // Přiřazení proměnných
        $this->template->skey = $skey;
        $this->template->id_senzoru = $id_senzoru;
        $this->template->teplota = $teplota;
        $this->template->vlhkost = $vlhkost;

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
        if ($this->skey && $this->skey == "t4m15")
            return true;
        return false;
    }

    private function pridatZaznam() {
        echo $this->skey;
        echo $this->id_senzoru;
        echo $this->teplota;
        echo $this->vlhkost;
    }
}

