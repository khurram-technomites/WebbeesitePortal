using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
	public class NumberRangeService : INumberRangeService
	{
		private readonly INumberRangeRepo _repo;

		public NumberRangeService(INumberRangeRepo repo)
		{
			_repo = repo;
		}

		public async Task<string> GetNextRange(string prefix)
		{
			string number = string.Empty;
			IEnumerable<NumberRange> List = await _repo.GetByIdAsync(x => x.Prefix == prefix);
			NumberRange range = List.FirstOrDefault();

			number = range.Prefix + (range.CurrentCount + 1).ToString().PadLeft(range.Padding, '0');

			range.CurrentCount += 1;

			await _repo.UpdateAsync(range);

			return number;
		}

		public async Task<string> GetNumberRangeByName(string name)
		{
			string number = string.Empty;
			IEnumerable<NumberRange> List = await _repo.GetByIdAsync(x => x.Name == name);
			NumberRange range = List.FirstOrDefault();

			number = range.Prefix + (range.CurrentCount + 1).ToString().PadLeft(range.Padding, '0');

			range.CurrentCount += 1;

			await _repo.UpdateAsync(range);

			return number;
		}
	}
}
