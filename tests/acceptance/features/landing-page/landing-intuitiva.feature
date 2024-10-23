Feature: Landing Page intuitiva

  Como visitante,
  Quiero poder navegar fácilmente por la landing page
  Para encontrar la información que necesito de forma rápida y sencilla.

  Scenario: Navegación correcta de la landing page
    Given que el visitante está en la landing page
    When el visitante hace clic en los elementos de navegación
    Then debería ser redirigido a las secciones correspondientes.

  Scenario: Sección no encontrada
    Given que el visitante está en la landing page
    When el visitante ingresa una URL incorrecta en la barra de direcciones
    Then debería ver un mensaje de "Página no encontrada".
