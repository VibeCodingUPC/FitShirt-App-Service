Feature: Creación de cuenta de empresa

  Como representante de una empresa,
  Quiero poder crear una cuenta de empresa
  Para administrar los servicios de mi empresa en la plataforma.

  Scenario: Creación exitosa de cuenta de empresa
    Given que el representante está en la página de registro de empresa
    When el representante ingresa los datos válidos de la empresa
    And hace clic en el botón "Registrar"
    Then debería ver un mensaje indicando "Cuenta de empresa creada exitosamente".

  Scenario: Creación fallida por datos incompletos
    Given que el representante está en la página de registro de empresa
    When el representante deja un campo obligatorio vacío
    And hace clic en el botón "Registrar"
    Then debería ver un mensaje indicando "Por favor complete todos los campos obligatorios".
