Feature: Creación de cuenta de cliente

  Como visitante,
  Quiero poder crear una cuenta de cliente
  Para acceder a las funcionalidades exclusivas para clientes.

  Scenario: Creación exitosa de cuenta de cliente
    Given que el visitante está en la página de registro de cliente
    When el visitante ingresa sus datos válidos en el formulario
    And hace clic en el botón "Registrar"
    Then debería ver un mensaje indicando "Cuenta creada exitosamente".

  Scenario: Creación fallida por datos incompletos
    Given que el visitante está en la página de registro de cliente
    When el visitante deja un campo obligatorio vacío
    And hace clic en el botón "Registrar"
    Then debería ver un mensaje indicando "Por favor complete todos los campos obligatorios".
