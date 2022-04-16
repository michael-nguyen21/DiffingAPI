using DiffingAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DiffingAPI.Controllers
{
    public class DiffController : ApiController
    {
        private DiffingAPIDbContext _context;
        public DiffController()
        {
            _context = new DiffingAPIDbContext();
        }
        // GET v1/diff/id
        public IHttpActionResult Get(int Id)
        {
            var diff = _context.Diff.SingleOrDefault(x => x.DiffId == Id);
            if (diff != null)
            {
                //If left and right have data, do comparison
                if (diff.Left != null && diff.Right != null) 
                {
                    //Left equal Right, return Ok and Equals
                    if (diff.Left == diff.Right) 
                    {
                        return Ok(new DiffResult { diffResultType = "Equals" });
                    }
                    
                    // Left != Right, convert to bytes and do comparison
                    else
                    {
                        byte[] leftBytes = Convert.FromBase64String(diff.Left);
                        byte[] rightBytes = Convert.FromBase64String(diff.Right);

                        //if their lengths are equal, compares each byte. Return OK, diffResultType and diffs
                        if (leftBytes.Length == rightBytes.Length) 
                        {
                            List<DiffPart> diffs = new List<DiffPart>();
                            for (int i = 0; i < leftBytes.Length; i++)
                            {
                                if (leftBytes[i] != rightBytes[i])
                                {
                                    if (diffs.Count > 0)
                                    {
                                        if (diffs.Last().Offset + diffs.Last().Length == i)
                                        {
                                            diffs.Last().Length++;
                                        }
                                        else
                                        {
                                            diffs.Add(new DiffPart { Offset = i, Length = 1 });
                                        }
                                    }
                                    else
                                    {
                                        diffs.Add(new DiffPart { Offset = i, Length = 1 });
                                    }

                                }
                            }
                            DiffResultWithDiffs diffResult = new DiffResultWithDiffs();
                            diffResult.diffResultType = "ContentDoNotMatch";
                            diffResult.diffs = diffs;
                            return Ok(diffResult);
                        }

                        // if their lengths are not equal, return OK and SizeDoNotMatch
                        else
                        {
                            return Ok(new DiffResult { diffResultType = "SizeDoNotMatch" });
                        }
                    }
                }
            }

            //If either left and right have not data, return 404 Not Found
            return NotFound();
        }

        // PUT v1/diff/id/pos
        [HttpPut]
        public IHttpActionResult Create(int Id, string pos, [FromBody] DiffData input )
        {
            //Data equal null, return 400 Bad Request
            if (input.data == null || input.data == "")
            {
                return BadRequest();
            }
            try
            {
                Convert.FromBase64String(input.data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
            var diff = _context.Diff.SingleOrDefault(x => x.DiffId == Id);
            
            // if diff by Id does not exist, creates new
            if (diff == null) 
            {
                diff = new Diff();
                diff.DiffId = Id;
                switch (pos.ToLower())
                {
                    case "left":
                        diff.Left = input.data;
                        break;
                    case "right":
                        diff.Right = input.data;
                        break;
                    default:
                        return BadRequest();
                }
                _context.Diff.Add(diff);
            }

            // if diff by Id exists, update it
            else
            {
                switch (pos.ToLower())
                {
                    case "left":
                        diff.Left = input.data;
                        break;
                    case "right":
                        diff.Right = input.data;
                        break;
                    default:
                        return BadRequest();
                }
            }

            _context.SaveChanges();
            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Created));
        }

       
    }
}
