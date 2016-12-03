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

    public function renderPridat($key = NULL, $sid = NULL, $tmp = NULL, $vum = NULL, $l = NULL) {
        $this->api->prirazeniParametru($key, $sid, $tmp, $vum, $l);

        // Přístup povolen, key zadán a zadán správně
        if ($this->api->overitPristup()) {
            $this->api->pridatZaznam();
        }
        // Nezadán security key, nepovolený přístup
        else {
            echo "Přístup odepřen!";
        }
    }

    public function renderReset($key = NULL, $sid = NULL, $tmp = NULL, $vum = NULL, $l = NULL) {
        $this->api->prirazeniParametru($key, $sid, $tmp, $vum, $l);

        // Přístup povolen, key zadán a zadán správně
        if ($this->api->overitPristup()) {
            $this->api->reset();
        }
        // Nezadán security key, nepovolený přístup
        else {
            echo "Přístup odepřen!";
        }
    }

    public function renderVyber($key = NULL, $sid = NULL, $tmp = NULL, $vum = NULL, $l = NULL, $pocet = NULL) {
        $this->api->prirazeniParametru($key, $sid, $tmp, $vum, $l, $pocet);

        // Přístup povolen, key zadán a zadán správně
        if ($this->api->overitPristup()) {
            echo $this->api->vybratZaznamy();
        }
        // Nezadán security key, nepovolený přístup
        else {
            echo "Přístup odepřen!";
        }
    }

    public function renderVenkovni($key = NULL, $sid = NULL, $tmp = NULL, $vum = NULL, $l = NULL) {
        $this->api->prirazeniParametru($key, $sid, $tmp, $vum, $l);

        // Přístup povolen, key zadán a zadán správně
        if ($this->api->overitPristup()) {
            echo $this->api->getOutDevice();
        }
        // Nezadán security key, nepovolený přístup
        else {
            echo "Přístup odepřen!";
        }
    }

    public function renderNastaveni($key = NULL, $nid = NULL, $maxtmp = NULL, $mintmp = NULL, $maxvum = NULL, $minvum = NULL, $topeni = NULL, $okna = NULL) {
        $this->api->prirazeniParametruNastaveni($key, $nid, $maxtmp, $mintmp, $maxvum, $minvum, $topeni, $okna);

        // Přístup povolen, key zadán a zadán správně
        if ($this->api->overitPristup()) {
            $this->api->update();
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
}
