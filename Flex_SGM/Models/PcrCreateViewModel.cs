using Flex_SGM.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Flex_SGM.ViewModels // Replace with your actual namespace
{
	public class PcrCreateViewModel
	{
		public pcr Pcr { get; set; }

		// Dropdown lists
		public IEnumerable<SelectListItem> Areas { get; set; }
		public IEnumerable<SelectListItem> Clientes { get; set; }
		public IEnumerable<SelectListItem> MatrizDecisions { get; set; }
		public IEnumerable<SelectListItem> Proyectos { get; set; }
		public IEnumerable<SelectListItem> Reasons { get; set; }
		public IEnumerable<SelectListItem> Gerentes { get; set; }
		public IEnumerable<SelectListItem> Maquinas { get; set; }

		// Additional properties if needed
		public string Originator { get; set; }
		public string Department { get; set; }
	}
}
