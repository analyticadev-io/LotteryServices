using BasicBackendTemplate.Dtos;
using BasicBackendTemplate.Interfaces;
using BasicBackendTemplate.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


namespace BasicBackendTemplate.Services
{
    public class ServiceModule : IModule
    {
        private readonly BasicBackendTemplateContext _context;

        public ServiceModule(BasicBackendTemplateContext context)
        {
            _context = context;
        }

        public async Task<Models.Module> AddModuleAsync(ResponseModule module)
        {
            // Verificar si el módulo ya existe en la base de datos
            bool moduleExists = await _context.Modules.AnyAsync(m => m.Name == module.module_name);

            if (moduleExists)
            {
                throw new InvalidOperationException("El módulo ya existe.");
            }

            var newModule = new Models.Module
            {
                Name = module.name,
                module_name = module.module_name,
                visibilityStatus="false",
                icon=module.icon,
            };

            var permissions = new List<Permiso>
            {
                new Permiso { Descripcion = $"browse_{module.module_name}" },
                new Permiso { Descripcion = $"read_{module.module_name}" },
                new Permiso { Descripcion = $"edit_{module.module_name}" },
                new Permiso { Descripcion = $"add_{module.module_name}" },
                new Permiso { Descripcion = $"delete_{module.module_name}" }
            };

            _context.Modules.Add(newModule);
            _context.Permisos.AddRange(permissions);
            await _context.SaveChangesAsync();

            // Devolver el módulo agregado
            return newModule;
        }


        public async Task<bool> DeleteModuleAsync(int module)
        {
            // Verificar si el módulo existe en la base de datos
            var existingModule = await _context.Modules
                .FirstOrDefaultAsync(m => m.IdModule==module);

            if (existingModule == null)
            {
                throw new InvalidOperationException("El módulo NO existe.");
            }

            
            var permissions = await _context.Permisos
                .Where(p => p.Descripcion.StartsWith($"browse_{existingModule.module_name}") ||
                            p.Descripcion.StartsWith($"read_{existingModule.module_name}") ||
                            p.Descripcion.StartsWith($"edit_{existingModule.module_name}") ||
                            p.Descripcion.StartsWith($"add_{existingModule.module_name}") ||
                            p.Descripcion.StartsWith($"delete_{existingModule.module_name}"))
                .ToListAsync();

            // Eliminar todos los permisos obtenidos
            _context.Permisos.RemoveRange(permissions);

            _context.Modules.Remove(existingModule);
            // Guardar los cambios de manera asíncrona
            await _context.SaveChangesAsync();

            // Devolver el módulo actualizado
            return true;
        }

        public async Task<Models.Module> EditModuleAsync(ResponseModule module)
        {
            // Verificar si el módulo existe en la base de datos
            var existingModule = await _context.Modules
                .FirstOrDefaultAsync(m => m.module_name == module.last_module_name);

            if (existingModule == null)
            {
                throw new InvalidOperationException("El módulo NO existe.");
            }

            // Validar que el nuevo nombre del módulo no esté vacío
            if (string.IsNullOrWhiteSpace(module.module_name))
            {
                throw new ArgumentException("El nuevo nombre del módulo no puede estar vacío.");
            }

            // Verificar que no haya otro módulo con el mismo nuevo nombre
            bool newNameExists = await _context.Modules.AnyAsync(m => m.Name == module.module_name && m.Name != module.last_module_name);

            if (newNameExists)
            {
                throw new InvalidOperationException("Ya existe un módulo con el nuevo nombre.");
            }

            // Actualizar el nombre del módulo
            existingModule.Name = module.name; // Asegúrate de actualizar el nombre con el nuevo valor
            existingModule.module_name = module.module_name; // Asegúrate de actualizar el nombre con el nuevo valor
            existingModule.icon = module.icon;
            existingModule.visibilityStatus = "false"; // Puedes ajustar esta lógica según sea necesario

            // Renombrar los permisos asociados al módulo
            var permissions = await _context.Permisos
                .Where(p => p.Descripcion.StartsWith($"browse_{module.last_module_name}") ||
                            p.Descripcion.StartsWith($"read_{module.last_module_name}") ||
                            p.Descripcion.StartsWith($"edit_{module.last_module_name}") ||
                            p.Descripcion.StartsWith($"add_{module.last_module_name}") ||
                            p.Descripcion.StartsWith($"delete_{module.last_module_name}"))
                .ToListAsync();

            foreach (var permission in permissions)
            {
                // Actualizar la descripción del permiso según corresponda
                if (permission.Descripcion.StartsWith($"browse_{module.last_module_name}"))
                    permission.Descripcion = $"browse_{module.module_name}";
                else if (permission.Descripcion.StartsWith($"read_{module.last_module_name}"))
                    permission.Descripcion = $"read_{module.module_name}";
                else if (permission.Descripcion.StartsWith($"edit_{module.last_module_name}"))
                    permission.Descripcion = $"edit_{module.module_name}";
                else if (permission.Descripcion.StartsWith($"add_{module.last_module_name}"))
                    permission.Descripcion = $"add_{module.module_name}";
                else if (permission.Descripcion.StartsWith($"delete_{module.last_module_name}"))
                    permission.Descripcion = $"delete_{module.module_name}";
            }

            // Guardar los cambios de manera asíncrona
            await _context.SaveChangesAsync();

            // Devolver el módulo actualizado
            return existingModule;
        }


        public async Task<IEnumerable<Models.Module>> GetModulesAsync()
        {
            return await _context.Modules.ToListAsync();
        }
    }
}
