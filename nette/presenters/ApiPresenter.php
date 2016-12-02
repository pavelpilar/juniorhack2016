<?php

/*
 * Tým číslo 15
 * API pro přidávání do databáze
 * Chráněno salt klíčem
 */

namespace App\Presenters;

use Nette,
    Nette\Http\Url,
    App\Model\API;

class ApiPresenter extends Nette\Application\UI\Presenter
{
    private $api;

    public function __construct(Nette\Database\Context $db) {
        $this->api = new API($db);
    }

    public function renderPridat($key = NULL, $sid = NULL, $tmp = NULL, $vum = NULL) {
        $this->api->prirazeniParametru($key, $sid, $tmp, $vum);

        // Přístup povolen, key zadán a zadán správně
        if ($this->api->overitPristup()) {
            $this->api->pridatZaznam();
        }
        // Nezadán security key, nepovolený přístup
        else {
            echo "Přístup odepřen!";
        }
    }

    public function renderReset($key = NULL, $sid = NULL, $tmp = NULL, $vum = NULL) {
        $this->api->prirazeniParametru($key, $sid, $tmp, $vum);
    }

    public function renderVyber($key = NULL, $sid = NULL, $tmp = NULL, $vum = NULL, $pocet = NULL) {
        $this->api->prirazeniParametru($key, $sid, $tmp, $vum, $pocet);

        // Přístup povolen, key zadán a zadán správně
        if ($this->api->overitPristup()) {
            echo $this->api->vybratZaznamy();
        }
        // Nezadán security key, nepovolený přístup
        else {
            echo "Přístup odepřen!";
        }
    }

    public function renderDefault()
    {
        echo "Přístup odepřen!";
    }

    /*
    private function ziskatZaznamy($pocet) {
        if (is_numeric($pocet))
            return $pocet;
        return "Hodnota parametru není číselná!";
    }*/
}
