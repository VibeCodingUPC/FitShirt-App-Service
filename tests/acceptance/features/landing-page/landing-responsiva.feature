Feature: Landing Page responsiva

  Como visitante,
  Quiero que la landing page se adapte a mi dispositivo
  Para poder ver la página correctamente desde cualquier lugar.

  Scenario: Adaptación correcta a diferentes dispositivos
    Given que el visitante accede a la landing page desde un dispositivo móvil
    When la landing page se carga
    Then debería ajustarse correctamente al tamaño de la pantalla del dispositivo.

  Scenario: Adaptación correcta a pantallas grandes
    Given que el visitante accede a la landing page desde un monitor de escritorio
    When la landing page se carga
    Then debería ajustarse correctamente al tamaño de la pantalla del monitor.
