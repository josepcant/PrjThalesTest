document.addEventListener('DOMContentLoaded', function () {
    // Elementos DOM
    const productoIdInput = document.getElementById('productoId');
    const btnBuscar = document.getElementById('btnBuscar');
    const btnLimpiar = document.getElementById('btnLimpiar');
    const productosBody = document.getElementById('productosBody');
    const mensaje = document.getElementById('mensaje');
    const loading = document.getElementById('loading');

    // Cargar todos los productos al iniciar
    cargarProductos();

    // Event listeners
    btnBuscar.addEventListener('click', function () {
        const id = productoIdInput.value.trim();
        if (id) {
            cargarProductoPorId(id);
        } else {
            cargarProductos();
        }
    });

    btnLimpiar.addEventListener('click', function () {
        productoIdInput.value = '';
        cargarProductos();
    });

    // También buscar al presionar Enter en el input
    productoIdInput.addEventListener('keypress', function (e) {
        if (e.key === 'Enter') {
            btnBuscar.click();
        }
    });

    // Función para cargar todos los productos
    function cargarProductos() {
        mostrarLoading(true);
        ocultarMensaje();

        fetch('/Productos/GetAll')
            .then(response => {
                if (!response.ok) {
                    throw new Error('Error al cargar los productos');
                }
                return response.json();
            })
            .then(productos => {
                renderizarProductos(productos);
                mostrarLoading(false);
            })
            .catch(error => {
                mostrarError(error.message);
                mostrarLoading(false);
            });
    }

    // Función para cargar un producto por ID
    function cargarProductoPorId(id) {
        mostrarLoading(true);
        ocultarMensaje();

        fetch(`/Productos/GetById?id=${id}`)
            .then(response => {
                if (response.status === 404) {
                    throw new Error(`Producto con ID ${id} no encontrado`);
                }
                if (!response.ok) {
                    throw new Error('Error al cargar el producto');
                }
                return response.json();
            })
            .then(producto => {
                renderizarProductos([producto]);
                mostrarLoading(false);
            })
            .catch(error => {
                mostrarError(error.message);
                mostrarLoading(false);
            });
    }

    // Función para renderizar productos en la tabla
    function renderizarProductos(productos) {
        productosBody.innerHTML = '';

        if (productos.length === 0) {
            mostrarMensaje('No se encontraron productos', 'info');
            return;
        }

        productos.forEach(producto => {
            const row = document.createElement('tr');
            row.style.cursor = 'pointer';

            // Obtener la primera imagen o usar un placeholder
            const imagenUrl = producto.images && producto.images.length > 0
                ? producto.images[0]
                : 'https://via.placeholder.com/50';

            row.innerHTML = `
                <td>${producto.id}</td>
                <td><img src="${imagenUrl}" alt="${producto.title}" class="img-thumbnail" style="width: 50px; height: 50px;"></td>
                <td>${producto.title}</td>
                <td>${producto.category ? producto.category.name : 'Sin categoría'}</td>
                <td>${producto.price.toFixed(2)}</td>
                <td>${producto.impuesto.toFixed(2)}</td>
                <td>${producto.precioConImpuesto.toFixed(2)}</td>
            `;

            // Agregar evento para mostrar detalles al hacer clic en la fila
            row.addEventListener('click', () => mostrarDetallesProducto(producto));

            productosBody.appendChild(row);
        });
    }

    // Función para mostrar detalles del producto en un modal
    function mostrarDetallesProducto(producto) {
        const modalBody = document.getElementById('detalleProductoBody');
        const modalTitle = document.getElementById('detalleProductoModalLabel');

        modalTitle.textContent = producto.title;

        // Preparar el carrusel de imágenes si hay más de una
        let imagenesHtml = '';
        if (producto.images && producto.images.length > 0) {
            if (producto.images.length === 1) {
                imagenesHtml = `
                    <div class="text-center mb-3">
                        <img src="${producto.images[0]}" alt="${producto.title}" class="img-fluid" style="max-height: 300px;">
                    </div>
                `;
            } else {
                // Crear un carrusel para múltiples imágenes
                imagenesHtml = `
                    <div id="carouselProducto" class="carousel slide mb-3" data-bs-ride="carousel">
                        <div class="carousel-inner">
                            ${producto.images.map((img, index) => `
                                <div class="carousel-item ${index === 0 ? 'active' : ''}">
                                    <img src="${img}" class="d-block w-100" alt="Imagen ${index + 1}">
                                </div>
                            `).join('')}
                        </div>
                        <button class="carousel-control-prev" type="button" data-bs-target="#carouselProducto" data-bs-slide="prev">
                            <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                            <span class="visually-hidden">Anterior</span>
                        </button>
                        <button class="carousel-control-next" type="button" data-bs-target="#carouselProducto" data-bs-slide="next">
                            <span class="carousel-control-next-icon" aria-hidden="true"></span>
                            <span class="visually-hidden">Siguiente</span>
                        </button>
                    </div>
                `;
            }
        }

        // Armar el contenido del modal
        modalBody.innerHTML = `
            ${imagenesHtml}
            <div class="row">
                <div class="col-md-6">
                    <h5>Información Básica</h5>
                    <p><strong>ID:</strong> ${producto.id}</p>
                    <p><strong>Título:</strong> ${producto.title}</p>
                    <p><strong>Categoría:</strong> ${producto.category ? producto.category.name : 'Sin categoría'}</p>
                    <p><strong>Precio:</strong> ${producto.price.toFixed(2)}</p>
                    <p><strong>Impuesto (19%):</strong> ${producto.impuesto.toFixed(2)}</p>
                    <p><strong>Precio con Impuesto:</strong> ${producto.precioConImpuesto.toFixed(2)}</p>
                </div>
                <div class="col-md-6">
                    <h5>Detalles</h5>
                    <p><strong>Descripción:</strong> ${producto.description}</p>
                    <p><strong>Fecha de Creación:</strong> ${new Date(producto.creationAt).toLocaleString()}</p>
                    <p><strong>Última Actualización:</strong> ${new Date(producto.updatedAt).toLocaleString()}</p>
                </div>
            </div>
        `;

        // Mostrar el modal
        const modal = new bootstrap.Modal(document.getElementById('detalleProductoModal'));
        modal.show();
    }

    // Funciones auxiliares para manejo de UI
    function mostrarLoading(show) {
        loading.classList.toggle('d-none', !show);
    }

    function mostrarMensaje(texto, tipo = 'info') {
        mensaje.textContent = texto;
        mensaje.className = `alert alert-${tipo}`;
        mensaje.classList.remove('d-none');
    }

    function mostrarError(texto) {
        mostrarMensaje(texto, 'danger');
    }

    function ocultarMensaje() {
        mensaje.classList.add('d-none');
    }
});